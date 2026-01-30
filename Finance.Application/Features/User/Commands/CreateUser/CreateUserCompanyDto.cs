
namespace Finance.Application.Features.User.Commands.CreateUser;

public sealed record CreateUserCompanyDto(
    Guid CompanyId,
    IReadOnlyCollection<Guid> RoleIds
);
