using Finance.Application.Common.Exceptions;
using Finance.Application.Common.Pagination;
using Finance.Application.Common.Sorting;
using Finance.Application.Contracts.User;
using Finance.Application.Features.User.Commands.CreateUser;
using Finance.Application.Features.User.Commands.UpdateUser;
using Finance.Application.Features.User.Queries.GetAllUsers;
using Finance.Application.Features.User.Queries.GetUserById;
using Finance.Application.Features.Users.Queries.GetAllUsers;
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
    // Create a user
    public async Task<Guid> CreateUserAsync(
    string email,
    string password,
    string fullName,
    IReadOnlyCollection<CreateUserCompanyDto> companies,
    CancellationToken ct)
    {
        if (companies is null || companies.Count == 0)
            throw new InvalidOperationException(
                "User must belong to at least one company");

        var exists = await userManager.FindByEmailAsync(email);
        if (exists is not null)
            throw new InvalidOperationException(
                "User with this email already exists");

        var companyIds = companies.Select(c => c.CompanyId).ToList();
        var roleIds = companies.SelectMany(c => c.RoleIds).Distinct().ToList();

        var validCompanies = await db.Set<Company>()
            .Where(x => companyIds.Contains(x.Id) && x.IsActive)
            .Select(x => x.Id)
            .ToListAsync(ct);

        if (validCompanies.Count != companyIds.Count)
            throw new NotFoundException("One or more companies not found");

        var validRoles = await db.Set<IdentityRole<Guid>>()
            .Where(x => roleIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(ct);

        if (validRoles.Count != roleIds.Count)
            throw new NotFoundException("One or more roles not found");

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = fullName,
            IsActive = true
        };

        await using var tx = await db.Database.BeginTransactionAsync(ct);

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(e => e.Description)));

        foreach (var company in companies)
        {
            db.Add(new UserCompany(user.Id, company.CompanyId));

            foreach (var roleId in company.RoleIds.Distinct())
            {
                db.Add(new UserCompanyRole(
                    user.Id,
                    company.CompanyId,
                    roleId));
            }
        }

        await db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return user.Id;
    }

    // Get All Users
    public async Task<PagedResult<UserListItemDto>> GetAllAsync(
        Guid? companyId,
        string? search,
        UserSortField sortBy,
        SortDirection sortDirection,
        int pageNumber,
        int pageSize,
        CancellationToken ct)
    {
        var query = db.Users.AsNoTracking();

        if (companyId.HasValue)
        {
            query = query.Where(u =>
                db.Set<UserCompany>()
                  .Any(uc => uc.UserId == u.Id && uc.CompanyId == companyId));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var value = search.Trim().ToLower();

            query = query.Where(u =>
                u.FullName.ToLower().Contains(value) ||
                u.Email.ToLower().Contains(value));
        }

        query = sortBy switch
        {
            UserSortField.Email => sortDirection == SortDirection.Asc
                ? query.OrderBy(x => x.Email)
                : query.OrderByDescending(x => x.Email),

            UserSortField.CreatedAt => sortDirection == SortDirection.Asc
                ? query.OrderBy(x => x.CreatedAt)
                : query.OrderByDescending(x => x.CreatedAt),

            _ => sortDirection == SortDirection.Asc
                ? query.OrderBy(x => x.FullName)
                : query.OrderByDescending(x => x.FullName)
        };

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserListItemDto(
                u.Id,
                u.FullName,
                u.Email,
                u.IsActive,
                u.CreatedAt))
            .ToListAsync(ct);

        return new PagedResult<UserListItemDto>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }


    // Get User By Id
    public async Task<UserDetailsDto> GetByIdAsync(
        Guid userId,
        CancellationToken ct)
    {
        var user = await db.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.FullName,
                u.IsActive,
                u.CreatedAt
            })
            .FirstOrDefaultAsync(ct);

        if (user is null)
            throw new NotFoundException("User not found");

        var companies = await (
            from uc in db.Set<UserCompany>().AsNoTracking()
            join c in db.Set<Company>() on uc.CompanyId equals c.Id
            where uc.UserId == userId
            select new UserCompanyDto(
                c.Id,
                c.Name,
                (
                    from ucr in db.Set<UserCompanyRole>()
                    join r in db.Set<IdentityRole<Guid>>() on ucr.RoleId equals r.Id
                    where ucr.UserId == userId
                       && ucr.CompanyId == c.Id
                    select new UserRoleDto(
                        r.Id,
                        r.Name!
                    )
                ).ToList()
            )
        ).ToListAsync(ct);


        return new UserDetailsDto(
            user.Id,
            user.Email,
            user.FullName,
            user.IsActive,
            user.CreatedAt,
            companies);
    }

    // Update User
    public async Task UpdateUserAsync(
    Guid userId,
    string fullName,
    bool isActive,
    IReadOnlyCollection<UpdateUserCompanyDto> companies,
    CancellationToken ct)
    {
        var user = await db.Users
            .FirstOrDefaultAsync(u => u.Id == userId, ct);

        if (user is null)
            throw new NotFoundException("User Not Found");

        user.FullName = fullName;
        user.IsActive = isActive;

        var existingCompanies = await db.Set<UserCompany>()
            .Where(x => x.UserId == userId)
            .ToListAsync(ct);

        var existingRoles = await db.Set<UserCompanyRole>()
            .Where(x => x.UserId == userId)
            .ToListAsync(ct);

        var desiredCompanyIds = companies.Select(x => x.CompanyId).ToHashSet();

        await using var tx = await db.Database.BeginTransactionAsync(ct);

        var companiesToRemove = existingCompanies
            .Where(x => !desiredCompanyIds.Contains(x.CompanyId))
            .ToList();

        db.RemoveRange(companiesToRemove);

        db.RemoveRange(
            existingRoles.Where(r =>
                companiesToRemove.Any(c => c.CompanyId == r.CompanyId))
        );

        foreach (var company in companies)
        {
            if (!existingCompanies.Any(c => c.CompanyId == company.CompanyId))
            {
                db.Add(new UserCompany(userId, company.CompanyId));
            }

            var existingRoleIds = existingRoles
                .Where(r => r.CompanyId == company.CompanyId)
                .Select(r => r.RoleId)
                .ToHashSet();

            var desiredRoleIds = company.RoleIds.ToHashSet();

            // ➕ Add roles
            var rolesToAdd = desiredRoleIds.Except(existingRoleIds);
            foreach (var roleId in rolesToAdd)
            {
                db.Add(new UserCompanyRole(userId, company.CompanyId, roleId));
            }

            // ➖ Remove roles
            var rolesToRemove = existingRoles
                .Where(r =>
                    r.CompanyId == company.CompanyId &&
                    !desiredRoleIds.Contains(r.RoleId))
                .ToList();

            db.RemoveRange(rolesToRemove);
        }

        await db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
    }

}
