using Finance.Application.Contracts.Auth;
using Finance.Application.Contracts.Account;
using Finance.Application.Contracts.Company;
using Finance.Application.Contracts.Accounting;
using Finance.Application.Contracts.User;
using Finance.Infrastructure.Identity;
using Finance.Infrastructure.Services.Auth;
using Finance.Infrastructure.Services.AccountService;
using Finance.Infrastructure.Services.CompanyService;
using Finance.Infrastructure.Services.MoveService;
using Finance.Infrastructure.Services.Users;
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
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IMoveService, MoveService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserManagementService, UserManagementService>();
        return services;
    }
}
