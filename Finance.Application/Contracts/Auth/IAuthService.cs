using Finance.Application.Features.Auth.Login;

namespace Finance.Application.Contracts.Auth;

public interface IAuthService
{
    Task<LoginResult> LoginAsync(
        string email,
        string password,
        Guid companyId,
        CancellationToken ct);
}
