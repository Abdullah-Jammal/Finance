namespace Finance.Application.Contracts.Accounting;

public sealed record MoveLineRequest(
    Guid AccountId,
    decimal Debit,
    decimal Credit,
    Guid? PartnerId);

public interface IMoveService
{
    Task<Guid> CreateAsync(
        Guid companyId,
        DateOnly date,
        IReadOnlyCollection<MoveLineRequest> lines,
        CancellationToken ct);
}
