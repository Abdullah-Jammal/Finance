using Finance.Application.Common.Exceptions;
using Finance.Application.Contracts.User;
using Finance.Domain.Entities.Auth;
using Finance.Domain.Entities.Company;
using Finance.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Finance.Infrastructure.Services.Users;

public sealed class UserManagementService(
    UserManager<ApplicationUser> userManager,
    FinanceDbContext db)
    : IUserManagementService
{
    public async Task AssignRoleAsync(
        Guid userId,
        Guid companyId,
        Guid roleId,
        CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            throw NotFoundException.ForEntity("User", userId);

        if (!user.IsActive)
            throw new InvalidOperationException("User is inactive");

        var companyExists = await db.Set<Company>()
            .AnyAsync(x => x.Id == companyId && x.IsActive, ct);

        if (!companyExists)
            throw NotFoundException.ForEntity("Company", companyId);

        var belongs = await db.Set<UserCompany>()
            .AnyAsync(x =>
                x.UserId == userId &&
                x.CompanyId == companyId,
                ct);

        if (!belongs)
            throw new InvalidOperationException(
                "User does not belong to this company");

        var roleExists = await db.Set<IdentityRole<Guid>>()
            .AnyAsync(x => x.Id == roleId, ct);

        if (!roleExists)
            throw NotFoundException.ForEntity("Role", roleId);

        var alreadyAssigned = await db.Set<UserCompanyRole>()
            .AnyAsync(x =>
                x.UserId == userId &&
                x.CompanyId == companyId &&
                x.RoleId == roleId,
                ct);

        if (alreadyAssigned)
            return;

        db.Add(new UserCompanyRole(
            userId,
            companyId,
            roleId));

        await db.SaveChangesAsync(ct);
    }

    public async Task<Guid> CreateUserAsync(
        string email,
        string password,
        string fullName,
        Guid companyId,
        Guid roleId,
        CancellationToken ct)
    {
        var exists = await userManager.FindByEmailAsync(email);
        if (exists is not null)
            throw new InvalidOperationException(
                "User with this email already exists");

        var companyExists = await db.Set<Company>()
            .AnyAsync(x => x.Id == companyId && x.IsActive, ct);

        if (!companyExists)
            throw NotFoundException.ForEntity("Company", companyId);

        var roleExists = await db.Set<IdentityRole<Guid>>()
            .AnyAsync(x => x.Id == roleId, ct);

        if (!roleExists)
            throw NotFoundException.ForEntity("Role", roleId);

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = fullName,
            IsActive = true
        };

        await using var transaction =
            await db.Database.BeginTransactionAsync(ct);

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new InvalidOperationException(
                string.Join(", ",
                    result.Errors.Select(e => e.Description)));

        db.Add(new UserCompany(user.Id, companyId));
        db.Add(new UserCompanyRole(user.Id, companyId, roleId));
        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        return user.Id;
    }

    public async Task AssignUserToCompanyAsync(
        Guid userId,
        Guid companyId,
        CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            throw NotFoundException.ForEntity("User", userId);

        if (!user.IsActive)
            throw new InvalidOperationException("User is inactive");

        var companyExists = await db.Set<Company>()
            .AnyAsync(x => x.Id == companyId && x.IsActive, ct);

        if (!companyExists)
            throw NotFoundException.ForEntity("Company", companyId);

        var alreadyAssigned = await db.Set<UserCompany>()
            .AnyAsync(x =>
                x.UserId == userId &&
                x.CompanyId == companyId,
                ct);

        if (alreadyAssigned)
            return;

        db.Add(new UserCompany(userId, companyId));
        await db.SaveChangesAsync(ct);
    }
}
