using Finance.Domain.Enums;

namespace Finance.Application.Contracts.Account;

public interface IAccountService
{
    Task<Guid> CreateAsync(
        Guid companyId,
        string code,
        string name,
        AccountType type,
        AccountSubtype subtype,
        Guid? parentId,
        bool isReconcilable,
        bool allowPosting,
        bool isActive,
        CancellationToken ct);
}
