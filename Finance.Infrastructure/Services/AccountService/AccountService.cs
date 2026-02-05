using Finance.Application.Common.Exceptions;
using Finance.Application.Contracts.Account;
using Finance.Domain.Entities.Accounting;
using Finance.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Finance.Infrastructure.Services.AccountService;

public sealed class AccountService(FinanceDbContext db) : IAccountService
{
    public async Task<Guid> CreateAsync(
        Guid companyId,
        string code,
        string name,
        AccountType type,
        string? subtype,
        Guid? parentId,
        bool isReconcilable,
        bool allowPosting,
        bool isActive,
        CancellationToken ct)
    {
        var exists = await db.Set<Account>()
            .AnyAsync(x => x.CompanyId == companyId && x.Code == code, ct);

        if (exists)
            throw new InvalidOperationException(
                $"Account with code '{code}' already exists for this company.");

        if (parentId is not null)
        {
            var parentExists = await db.Set<Account>()
                .AnyAsync(
                    x => x.Id == parentId && x.CompanyId == companyId,
                    ct);

            if (!parentExists)
                throw new NotFoundException("Parent account not found.");
        }

        var account = new Account(
            companyId,
            code,
            name,
            type,
            subtype,
            parentId,
            isReconcilable,
            allowPosting,
            isActive);

        db.Add(account);
        await db.SaveChangesAsync(ct);

        return account.Id;
    }
}
