using Finance.Application.Common.Authorization;

namespace Finance.API.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(
        this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                Permissions.Companies_View,
                p => p.RequireClaim("permission", Permissions.Companies_View));

            options.AddPolicy(
                Permissions.Companies_Create,
                p => p.RequireClaim("permission", Permissions.Companies_Create));

            options.AddPolicy(
                Permissions.Companies_Update,
                p => p.RequireClaim("permission", Permissions.Companies_Update));

            options.AddPolicy(
                Permissions.Companies_Delete,
                p => p.RequireClaim("permission", Permissions.Companies_Delete));

            options.AddPolicy(
                Permissions.Accounts_Create,
                p => p.RequireClaim("permission", Permissions.Accounts_Create));

            options.AddPolicy(
                Permissions.Users_View,
                p => p.RequireClaim("permission", Permissions.Users_View));

            options.AddPolicy(
                Permissions.Users_Manage,
                p => p.RequireClaim("permission", Permissions.Users_Manage));

            options.AddPolicy(
                Permissions.Roles_Manage,
                p => p.RequireClaim("permission", Permissions.Roles_Manage));
        });

        return services;
    }
}
