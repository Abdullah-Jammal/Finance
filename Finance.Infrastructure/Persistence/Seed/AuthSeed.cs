using Finance.Application.Common.Authorization;
using Finance.Domain.Entities.Auth;
using Finance.Domain.Entities.Company;
using Finance.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Finance.Infrastructure.Persistence.Seed;

public static class AuthSeed
{
    public static async Task SeedAsync(
        FinanceDbContext db,
        RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        foreach (var roleName in new[]
        {
            Roles.SuperAdmin,
            Roles.CompanyAdmin,
            Roles.Accountant,
            Roles.Viewer
        })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(
                    new IdentityRole<Guid>(roleName));
            }
        }

        var permissions = new[]
        {
            Permissions.Companies_View,
            Permissions.Companies_Create,
            Permissions.Companies_Update,
            Permissions.Companies_Delete,
            Permissions.Accounts_Create,
            Permissions.Users_View,
            Permissions.Users_Manage,
            Permissions.Roles_Manage
        };

        foreach (var code in permissions)
        {
            if (!await db.Set<Permission>().AnyAsync(p => p.Code == code))
            {
                db.Add(new Permission(code));
            }
        }

        await db.SaveChangesAsync();

        var superAdminRole =
            await roleManager.FindByNameAsync(Roles.SuperAdmin);

        var permissionIds =
            await db.Set<Permission>().Select(p => p.Id).ToListAsync();

        foreach (var permissionId in permissionIds)
        {
            var exists = await db.Set<RolePermission>()
                .AnyAsync(x =>
                    x.RoleId == superAdminRole!.Id &&
                    x.PermissionId == permissionId);

            if (!exists)
            {
                db.Add(new RolePermission(
                    superAdminRole!.Id,
                    permissionId));
            }
        }

        await db.SaveChangesAsync();

        var company = await db.Set<Company>()
            .FirstOrDefaultAsync(x => x.Code == "DEFAULT");

        if (company is null)
        {
            company = new Company(
                "Default Company",
                "DEFAULT",
                "USD");
            db.Add(company);
            await db.SaveChangesAsync();
        }

        var adminEmail = "admin@finance.local";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "System Administrator",
                IsActive = true
            };

            var createResult = await userManager.CreateAsync(
                adminUser,
                "Admin123!");

            if (!createResult.Succeeded)
                throw new InvalidOperationException(
                    string.Join(", ",
                        createResult.Errors.Select(e => e.Description)));
        }

        var userCompanyExists = await db.Set<UserCompany>()
            .AnyAsync(x =>
                x.UserId == adminUser.Id &&
                x.CompanyId == company.Id);

        if (!userCompanyExists)
        {
            db.Add(new UserCompany(adminUser.Id, company.Id));
        }

        var userRoleExists = await db.Set<UserCompanyRole>()
            .AnyAsync(x =>
                x.UserId == adminUser.Id &&
                x.CompanyId == company.Id &&
                x.RoleId == superAdminRole!.Id);

        if (!userRoleExists)
        {
            db.Add(new UserCompanyRole(
                adminUser.Id,
                company.Id,
                superAdminRole!.Id));
        }

        await db.SaveChangesAsync();
    }
}
