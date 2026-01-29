namespace Finance.Application.Features.Auth.SelectCompany;

public sealed record LoginResult
{
    public required string AccessToken { get; init; }
    public required DateTime ExpiresAt { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTime RefreshTokenExpiresAt { get; init; }
}
