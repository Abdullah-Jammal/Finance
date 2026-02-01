namespace Finance.Application.Contracts.Account;

public interface IAccountService
{
    Task<Guid> CreateAsync(
        Guid companyId,
        string code,
        string name,
        string type,
        string? subtype,
        Guid? parentId,
        bool isReconcilable,
        bool allowPosting,
        bool isActive,
        CancellationToken ct);
}
