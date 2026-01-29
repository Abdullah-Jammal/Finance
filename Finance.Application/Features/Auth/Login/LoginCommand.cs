using MediatR;

namespace Finance.Application.Features.Auth.Login;

public sealed record LoginCommand(
    string Email,
    string Password
) : IRequest<LoginCompaniesResult>;
