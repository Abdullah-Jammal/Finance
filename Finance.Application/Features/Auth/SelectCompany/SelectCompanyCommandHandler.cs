using Finance.Application.Contracts.Auth;
using MediatR;

namespace Finance.Application.Features.Auth.SelectCompany;

public sealed class SelectCompanyCommandHandler(IAuthService authService)
    : IRequestHandler<SelectCompanyCommand, LoginResult>
{
    public Task<LoginResult> Handle(
        SelectCompanyCommand request,
        CancellationToken ct)
    {
        return authService.SelectCompanyAsync(
            request.UserId,
            request.CompanyId,
            ct);
    }
}
