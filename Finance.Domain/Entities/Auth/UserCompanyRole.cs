
namespace Finance.Domain.Entities.Auth;

public class UserCompanyRole
{
    public Guid UserId { get; private set; }
    public Guid CompanyId { get; private set; }
    public Guid RoleId { get; private set; }

    private UserCompanyRole() { }

    public UserCompanyRole(Guid userId, Guid companyId, Guid roleId)
    {
        UserId = userId;
        CompanyId = companyId;
        RoleId = roleId;
    }
}
