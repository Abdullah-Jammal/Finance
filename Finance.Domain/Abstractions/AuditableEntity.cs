namespace Finance.Domain.Abstractions;

public abstract class AuditableEntity<TId> : EntityBase<TId>
{
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public void Touch() => UpdatedAt = DateTime.UtcNow;
}
