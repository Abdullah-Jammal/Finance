using Finance.Application.Contracts.User;
using MediatR;

namespace Finance.Application.Features.User.Commands.CreateUser;

public sealed class CreateUserCommandHandler(
    IUserManagementService service)
    : IRequestHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateUserCommand request,
        CancellationToken ct)
    {
        return await service.CreateUserAsync(
            request.Email,
            request.Password,
            request.FullName,
            request.Companies,
            ct);
    }
}
