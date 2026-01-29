using MediatR;

namespace Finance.Application.Features.User.Commands.CreateUser;

public sealed record CreateUserCommand(
    string Email,
    string Password,
    string FullName
) : IRequest<Guid>;