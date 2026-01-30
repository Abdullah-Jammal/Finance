
namespace Finance.Application.Features.User.Commands.UpdateUser;

public sealed record UpdateUserRequestDto(
    string FullName,
    bool IsActive,
    IReadOnlyCollection<UpdateUserCompanyDto> Companies
);
