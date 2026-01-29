using Finance.Application.Features.Auth.Login;
using MediatR;

namespace Finance.Application.Features.Auth.Refresh;

public sealed record RefreshTokenCommand(string RefreshToken)
    : IRequest<LoginResult>;
