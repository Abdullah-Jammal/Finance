using Finance.Application.Contracts.Auth;
using Finance.Application.Features.Auth.Login;
using MediatR;

namespace Finance.Application.Features.Auth.Refresh;

public sealed class RefreshTokenCommandHandler(IAuthService authService)
    : IRequestHandler<RefreshTokenCommand, LoginResult>
{
    public Task<LoginResult> Handle(
        RefreshTokenCommand request,
        CancellationToken ct)
    {
        return authService.RefreshTokenAsync(
            request.RefreshToken,
            ct);
    }
}
