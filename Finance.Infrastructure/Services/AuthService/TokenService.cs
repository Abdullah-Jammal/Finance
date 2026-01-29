using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Finance.Application.Contracts.Auth;
using Finance.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Finance.Infrastructure.Services.AuthService;

public sealed class TokenService(
    FinanceDbContext db,
    IConfiguration configuration) : ITokenService
{
    public async Task<string> GenerateTokenAsync(
    Guid userId,
    string email,
    Guid companyId,
    CancellationToken ct)
    {
        var permissions = await (
            from ucr in db.Set<UserCompanyRole>()
            join rp in db.Set<RolePermission>()
                on ucr.RoleId equals rp.RoleId
            join p in db.Set<Permission>()
                on rp.PermissionId equals p.Id
            where ucr.UserId == userId
               && ucr.CompanyId == companyId
            select p.Code
        ).Distinct().ToListAsync(ct);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new("company_id", companyId.ToString())
        };

        claims.AddRange(
            permissions.Select(p =>
                new Claim("permission", p)));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                configuration["Jwt:Key"]!));

        var creds = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}
