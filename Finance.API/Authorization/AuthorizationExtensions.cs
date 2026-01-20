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
        });

        return services;
    }
}
