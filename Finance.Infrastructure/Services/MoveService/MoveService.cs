using Finance.Application.Common.Exceptions;
using Finance.Application.Contracts.Accounting;
using Finance.Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;

namespace Finance.Infrastructure.Services.MoveService;

public sealed class MoveService(FinanceDbContext db) : IMoveService
{
    public async Task<Guid> CreateAsync(
        Guid companyId,
        DateOnly date,
        IReadOnlyCollection<MoveLineRequest> lines,
        CancellationToken ct)
    {
        var accountIds = lines
            .Select(line => line.AccountId)
            .Distinct()
            .ToList();

        var accounts = await db.Set<Account>()
            .Where(a => a.CompanyId == companyId && accountIds.Contains(a.Id))
            .ToDictionaryAsync(a => a.Id, ct);

        if (accounts.Count != accountIds.Count)
            throw new NotFoundException("One or more accounts not found.");

        var move = new Move(companyId, date);

        foreach (var line in lines)
        {
            var account = accounts[line.AccountId];

            move.AddLine(
                new MoveLine(
                    account,
                    line.Debit,
                    line.Credit,
                    line.PartnerId));
        }

        move.Post();

        db.Add(move);
        await db.SaveChangesAsync(ct);

        return move.Id;
    }
}
