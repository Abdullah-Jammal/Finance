using Microsoft.AspNetCore.Authorization;

namespace Finance.API.Authorization;

public sealed class PermissionRequirement(string permission)
    : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
