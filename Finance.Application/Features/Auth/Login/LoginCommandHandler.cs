using Finance.Application.Contracts.Auth;
using MediatR;

namespace Finance.Application.Features.Auth.Login;

public sealed class LoginCommandHandler(IAuthService authService)
    : IRequestHandler<LoginCommand, LoginCompaniesResult>
{
    public Task<LoginCompaniesResult> Handle(
        LoginCommand request,
        CancellationToken ct)
    {
        return authService.LoginAsync(
            request.Email,
            request.Password,
            ct);
    }
}
