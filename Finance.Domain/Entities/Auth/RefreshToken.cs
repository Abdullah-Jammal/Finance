namespace Finance.Domain.Entities.Auth;

public sealed class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid CompanyId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private RefreshToken() { }

    public RefreshToken(
        Guid userId,
        Guid companyId,
        string token,
        DateTime expiresAt)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CompanyId = companyId;
        Token = token;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
    }

    public bool IsActive => RevokedAt is null && ExpiresAt > DateTime.UtcNow;

    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }
}
