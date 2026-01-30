using MediatR;

namespace Finance.Application.Features.User.Commands.UpdateUser;

public sealed record UpdateUserCommand(
    Guid UserId,
    string FullName,
    bool IsActive,
    IReadOnlyCollection<UpdateUserCompanyDto> Companies
) : IRequest;
