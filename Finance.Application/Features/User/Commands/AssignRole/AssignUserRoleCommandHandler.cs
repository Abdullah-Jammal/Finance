using Finance.Application.Contracts.User;
using MediatR;

namespace Finance.Application.Features.User.Commands.AssignRole;

public sealed class AssignUserRoleCommandHandler(
    IUserManagementService service)
    : IRequestHandler<AssignUserRoleCommand>
{
    public async Task Handle(
        AssignUserRoleCommand request,
        CancellationToken ct)
    {
        await service.AssignRoleAsync(
            request.UserId,
            request.CompanyId,
            request.RoleId,
            ct);
    }
}
