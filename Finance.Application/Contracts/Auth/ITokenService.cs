namespace Finance.Application.Contracts.Auth;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(
        Guid userId,
        string email,
        Guid companyId,
        CancellationToken ct);
}
