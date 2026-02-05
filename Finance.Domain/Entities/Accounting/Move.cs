using Finance.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Finance.Domain.Entities.Accounting;

public sealed class Move : AuditableEntity<Guid>
{
    public Guid CompanyId { get; private set; }
    public DateOnly Date { get; private set; }
    public bool IsPosted { get; private set; }

    private readonly List<MoveLine> _lines = new();
    public IReadOnlyCollection<MoveLine> Lines => _lines;

    private Move() { }

    public Move(Guid companyId, DateOnly date)
    {
        CompanyId = companyId;
        Date = date;
        IsPosted = false;
    }

    public void AddLine(MoveLine line)
    {
        if (IsPosted)
            throw new InvalidOperationException("Cannot modify a posted move.");

        _lines.Add(line);
    }

    public void Post()
    {
        EnsureBalanced();
        EnsurePostable();

        IsPosted = true;
        Touch();
    }

    private void EnsureBalanced()
    {
        var debit = _lines.Sum(l => l.Debit);
        var credit = _lines.Sum(l => l.Credit);

        if (debit != credit)
            throw new InvalidOperationException("Move is not balanced.");
    }

    private void EnsurePostable()
    {
        if (_lines.Count < 2)
            throw new InvalidOperationException("Move must have at least two lines.");
    }
}
