using Finance.Application.Features.Auth.SelectCompany;
using MediatR;

namespace Finance.Application.Features.Auth.Refresh;

public sealed record RefreshTokenCommand(string RefreshToken)
    : IRequest<LoginResult>;
