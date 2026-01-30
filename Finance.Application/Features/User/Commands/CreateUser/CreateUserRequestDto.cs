
namespace Finance.Application.Features.User.Commands.CreateUser;

public sealed record CreateUserRequestDto(
    string Email,
    string Password,
    string FullName,
    IReadOnlyCollection<CreateUserCompanyRequest> Companies
);

public sealed record CreateUserCompanyRequest(
    Guid CompanyId,
    IReadOnlyCollection<Guid> RoleIds
);
