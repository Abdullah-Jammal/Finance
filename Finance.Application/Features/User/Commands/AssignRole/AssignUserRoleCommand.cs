using MediatR;

namespace Finance.Application.Features.User.Commands.AssignRole;

public sealed record AssignUserRoleCommand(
    Guid UserId,
    Guid CompanyId,
    Guid RoleId
) : IRequest;
