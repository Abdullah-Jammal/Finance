using Finance.Application.Contracts.Auth;
using Finance.Application.Features.Auth.Login;
using Finance.Application.Features.Auth.SelectCompany;
using Finance.Domain.Entities.Auth;
using Finance.Domain.Entities.Company;
using Finance.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Finance.Infrastructure.Services.Auth;

public sealed class AuthService(
    UserManager<ApplicationUser> userManager,
    FinanceDbContext db,
    ITokenService tokenService)
    : IAuthService
{
    public async Task<LoginCompaniesResult> LoginAsync(
        string email,
        string password,
        CancellationToken ct)
    {
        // 1️⃣ Find user
        var user = await userManager.FindByEmailAsync(email);
        if (user is null || !user.IsActive)
            throw new UnauthorizedAccessException("Invalid credentials");

        // 2️⃣ Check password
        var validPassword =
            await userManager.CheckPasswordAsync(user, password);

        if (!validPassword)
            throw new UnauthorizedAccessException("Invalid credentials");

        // 3️⃣ Get companies
        var companies = await (
            from uc in db.Set<UserCompany>()
            join company in db.Set<Company>()
                on uc.CompanyId equals company.Id
            where uc.UserId == user.Id
               && company.IsActive
            select new LoginCompanyDto
            {
                Id = company.Id,
                Name = company.Name
            }).ToListAsync(ct);

        if (companies.Count == 0)
            throw new UnauthorizedAccessException(
                "User is not assigned to any company");

        var fullName = string.IsNullOrWhiteSpace(user.FullName)
            ? user.Email ?? string.Empty
            : user.FullName;

        // 4️⃣ Issue TEMP token
        var tempToken = tokenService.CreateTempToken(user.Id);

        return new LoginCompaniesResult
        {
            UserId = user.Id,
            FullName = fullName,
            TempToken = tempToken,
            Companies = companies
        };
    }


    public async Task<LoginResult> SelectCompanyAsync(
        Guid userId,
        Guid companyId,
        CancellationToken ct)
    {
        var user = await userManager.Users
            .FirstOrDefaultAsync(x => x.Id == userId, ct);

        if (user is null || !user.IsActive)
            throw new UnauthorizedAccessException("Invalid credentials");

        var belongsToCompany = await db.Set<UserCompany>()
            .AnyAsync(x =>
                x.UserId == userId &&
                x.CompanyId == companyId,
                ct);

        if (!belongsToCompany)
            throw new UnauthorizedAccessException(
                "User is not assigned to this company");

        var hasRoles = await db.Set<UserCompanyRole>()
            .AnyAsync(x =>
                x.UserId == userId &&
                x.CompanyId == companyId,
                ct);

        if (!hasRoles)
            throw new UnauthorizedAccessException(
                "User has no roles assigned for this company");

        return await IssueTokensAsync(user.Id, user.Email!, companyId, ct);
    }

    public async Task<LoginResult> RefreshTokenAsync(
        string refreshToken,
        CancellationToken ct)
    {
        var storedToken = await db.Set<RefreshToken>()
            .FirstOrDefaultAsync(
                x => x.Token == refreshToken,
                ct);

        if (storedToken is null || !storedToken.IsActive)
            throw new UnauthorizedAccessException("Invalid refresh token");

        storedToken.Revoke();

        var newRefreshTokenValue = GenerateRefreshToken();
        var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
        var newRefreshToken = new RefreshToken(
            storedToken.UserId,
            storedToken.CompanyId,
            newRefreshTokenValue,
            refreshTokenExpiresAt);

        db.Set<RefreshToken>().Add(newRefreshToken);

        var accessToken = await tokenService.GenerateTokenAsync(
            storedToken.UserId,
            await GetUserEmailAsync(storedToken.UserId, ct),
            storedToken.CompanyId,
            ct);

        await db.SaveChangesAsync(ct);

        return new LoginResult
        {
            AccessToken = accessToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            RefreshToken = newRefreshTokenValue,
            RefreshTokenExpiresAt = refreshTokenExpiresAt
        };
    }

    private async Task<LoginResult> IssueTokensAsync(
        Guid userId,
        string email,
        Guid companyId,
        CancellationToken ct)
    {
        var token = await tokenService.GenerateTokenAsync(
            userId,
            email,
            companyId,
            ct);

        var refreshTokenValue = GenerateRefreshToken();
        var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
        var refreshToken = new RefreshToken(
            userId,
            companyId,
            refreshTokenValue,
            refreshTokenExpiresAt);

        db.Set<RefreshToken>().Add(refreshToken);
        await db.SaveChangesAsync(ct);

        return new LoginResult
        {
            AccessToken = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            RefreshToken = refreshTokenValue,
            RefreshTokenExpiresAt = refreshTokenExpiresAt
        };
    }

    private static string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    private async Task<string> GetUserEmailAsync(Guid userId, CancellationToken ct)
    {
        var user = await userManager.Users
            .Where(x => x.Id == userId)
            .Select(x => x.Email)
            .FirstOrDefaultAsync(ct);

        if (string.IsNullOrWhiteSpace(user))
            throw new UnauthorizedAccessException("Invalid refresh token");

        return user;
    }
}
