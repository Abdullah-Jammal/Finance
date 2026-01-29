using Finance.Application.Features.Auth.Login;

namespace Finance.Application.Contracts.Auth;

public interface IAuthService
{
    Task<LoginCompaniesResult> LoginAsync(
        string email,
        string password,
        CancellationToken ct);

    Task<LoginResult> SelectCompanyAsync(
        Guid userId,
        Guid companyId,
        CancellationToken ct);

    Task<LoginResult> RefreshTokenAsync(
        string refreshToken,
        CancellationToken ct);
}
