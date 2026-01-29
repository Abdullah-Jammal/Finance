using Finance.Application.Contracts.Auth;
using Finance.Application.Features.Auth.Login;
using Finance.Domain.Entities.Auth;
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
    public async Task<LoginResult> LoginAsync(
        string email,
        string password,
        Guid companyId,
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

        // 3️⃣ Check company membership
        var belongsToCompany = await db.Set<UserCompany>()
            .AnyAsync(x =>
                x.UserId == user.Id &&
                x.CompanyId == companyId,
                ct);

        if (!belongsToCompany)
            throw new UnauthorizedAccessException(
                "User is not assigned to this company");

        // 4️⃣ Generate JWT
        var token = await tokenService.GenerateTokenAsync(
            user.Id,
            user.Email!,
            companyId,
            ct);

        var refreshTokenValue = GenerateRefreshToken();
        var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
        var refreshToken = new RefreshToken(
            user.Id,
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
