using Finance.Application.Common.Authorization;
using Finance.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Finance.Infrastructure.Persistence.Seed;

public static class AuthSeed
{
    public static async Task SeedAsync(
        FinanceDbContext db,
        RoleManager<IdentityRole<Guid>> roleManager)
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
    }
}
