using Finance.Application.Contracts.Auth;
using Finance.Application.Features.Auth.Login;
using Finance.Domain.Entities.Auth;
using Finance.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        return new LoginResult
        {
            AccessToken = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };
    }
}
