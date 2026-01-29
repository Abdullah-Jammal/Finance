using MediatR;

namespace Finance.Application.Features.Auth.Login;

public sealed record LoginCommand(
    string Email,
    string Password,
    Guid CompanyId
) : IRequest<LoginResult>;
