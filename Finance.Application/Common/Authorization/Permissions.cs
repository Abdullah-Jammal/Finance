namespace Finance.Application.Common.Authorization;

public static class Permissions
{
    public const string Companies_View = "companies.view";
    public const string Companies_Create = "companies.create";
    public const string Companies_Update = "companies.update";
    public const string Companies_Delete = "companies.delete";

    public const string Accounts_Create = "accounts.create";

    public const string Users_View = "users.view";
    public const string Users_Manage = "users.manage";

    public const string Roles_Manage = "roles.manage";
}
