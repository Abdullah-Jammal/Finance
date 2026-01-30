using Finance.Application.Contracts.User;
using MediatR;

namespace Finance.Application.Features.User.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler(
    IUserManagementService service)
    : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(
        UpdateUserCommand request,
        CancellationToken ct)
    {
        await service.UpdateUserAsync(
            request.UserId,
            request.FullName,
            request.IsActive,
            request.Companies,
            ct);
    }
}