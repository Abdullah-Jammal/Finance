using System;

namespace Finance.Domain.Entities.Accounting;

public sealed class MoveLine
{
    public Guid AccountId { get; private set; }
    public Account Account { get; private set; } = default!;

    public decimal Debit { get; private set; }
    public decimal Credit { get; private set; }

    public Guid? PartnerId { get; private set; }

    private MoveLine() { }

    public MoveLine(
        Account account,
        decimal debit,
        decimal credit,
        Guid? partnerId)
    {
        if (debit < 0 || credit < 0)
            throw new ArgumentException("Debit and credit must be positive.");

        if (debit > 0 && credit > 0)
            throw new ArgumentException("Line cannot have both debit and credit.");

        if (debit == 0 && credit == 0)
            throw new ArgumentException("Line must have debit or credit.");

        account.EnsureCanPost();
        account.EnsurePartnerRequirement(partnerId);

        Account = account;
        AccountId = account.Id;
        Debit = debit;
        Credit = credit;
        PartnerId = partnerId;
    }
}
