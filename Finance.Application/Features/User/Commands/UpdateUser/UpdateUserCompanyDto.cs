
namespace Finance.Application.Features.User.Commands.UpdateUser;

public sealed record UpdateUserCompanyDto(
    Guid CompanyId,
    IReadOnlyCollection<Guid> RoleIds
);