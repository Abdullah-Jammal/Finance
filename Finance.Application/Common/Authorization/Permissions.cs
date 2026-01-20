namespace Finance.Application.Common.Authorization;

public static class Permissions
{
    // Companies
    public const string Companies_View = "companies.view";
    public const string Companies_Create = "companies.create";
    public const string Companies_Update = "companies.update";
    public const string Companies_Delete = "companies.delete";

    // Users
    public const string Users_View = "users.view";
    public const string Users_Manage = "users.manage";

    // Roles
    public const string Roles_Manage = "roles.manage";
}