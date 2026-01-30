
namespace Finance.Application.Features.User.Queries.GetUserById;

public sealed record UserDetailsDto(
    Guid Id,
    string Email,
    string FullName,
    bool IsActive,
    DateTime CreatedAt,
    IReadOnlyCollection<UserCompanyDto> Companies
);

public sealed record UserCompanyDto(
    Guid CompanyId,
    string CompanyName,
    IReadOnlyCollection<UserRoleDto> Roles
);

public sealed record UserRoleDto(
    Guid RoleId,
    string RoleName
);
