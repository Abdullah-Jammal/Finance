using Finance.Domain.Abstractions;
using Finance.Domain.Enums;
using Finance.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Finance.Domain.Entities.Accounting;

public class Account : AuditableEntity<Guid>
{
    public Guid CompanyId { get; private set; }
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public AccountType Type { get; private set; }
    public AccountSubtype Subtype { get; private set; }
    public Guid? ParentId { get; private set; }
    public Account? Parent { get; private set; }
    public IReadOnlyCollection<Account> Children => _children;
    public bool IsReconcilable { get; private set; }
    public bool RequiresPartner { get; private set; }
    public bool AllowPosting { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<Account> _children = new();

    private Account() { }

    public Account(
        Guid companyId,
        string code,
        string name,
        AccountType type,
        AccountSubtype subtype = AccountSubtype.None,
        Guid? parentId = null,
        bool allowPosting = true,
        bool isActive = true)
    {
        UpdateDetails(
            companyId,
            code,
            name,
            type,
            subtype,
            parentId,
            allowPosting,
            isActive);
    }

    public void UpdateDetails(
        Guid companyId,
        string code,
        string name,
        AccountType type,
        AccountSubtype subtype,
        Guid? parentId,
        bool allowPosting,
        bool isActive)
    {
        if (companyId == Guid.Empty)
            throw new ArgumentException("Company is required");

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Account code is required");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Account name is required");

        if (!Enum.IsDefined(typeof(AccountType), type))
            throw new ArgumentException("Account type is required");

        if (!Enum.IsDefined(typeof(AccountSubtype), subtype))
            throw new ArgumentException("Account subtype is required");

        if (!AccountSubtypeRules.IsValid(type, subtype))
            throw new ArgumentException("Account subtype is not valid for the account type");

        CompanyId = companyId;
        Code = code.Trim();
        Name = name.Trim();
        Type = type;
        Subtype = subtype;
        ParentId = parentId;
        IsReconcilable = AccountBehaviorRules.IsReconcilable(subtype);
        RequiresPartner = AccountBehaviorRules.RequiresPartner(subtype);
        AllowPosting = allowPosting && AccountBehaviorRules.AllowsManualPosting(subtype);
        IsActive = isActive;
        Touch();
    }

    public bool CanPost()
    {
        if (!AllowPosting)
            return false;

        if (_children.Any())
            return false;

        return true;
    }

    public void EnsureCanPost()
    {
        if (!CanPost())
            throw new InvalidOperationException(
                "Posting is not allowed on parent or group accounts.");
    }

    public void EnsurePartnerRequirement(Guid? partnerId)
    {
        if (RequiresPartner && partnerId is null)
            throw new InvalidOperationException(
                $"Account '{Code} - {Name}' requires a partner.");
    }
}
