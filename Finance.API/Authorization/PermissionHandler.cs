using Microsoft.AspNetCore.Authorization;

namespace Finance.API.Authorization;

public sealed class PermissionHandler
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var hasPermission = context.User
            .Claims
            .Any(c =>
                c.Type == "permission" &&
                c.Value == requirement.Permission);

        if (hasPermission)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
