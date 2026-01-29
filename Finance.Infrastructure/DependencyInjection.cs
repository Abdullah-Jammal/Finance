using Finance.Application.Contracts.Auth;
using Finance.Application.Contracts.Company;
using Finance.Infrastructure.Identity;
using Finance.Infrastructure.Services.Auth;
using Finance.Infrastructure.Services.AuthService;
using Finance.Infrastructure.Services.CompanyService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<FinanceDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<FinanceDbContext>()
        .AddDefaultTokenProviders();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
}
