namespace Finance.Application.Features.Auth.Login;

public sealed record LoginResult
{
    public required string AccessToken { get; init; }
    public required DateTime ExpiresAt { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTime RefreshTokenExpiresAt { get; init; }
}
