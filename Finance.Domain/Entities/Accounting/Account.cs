using Finance.Domain.Abstractions;
using Finance.Domain.Enums;
using Finance.Domain.Rules;

namespace Finance.Domain.Entities.Accounting;

public class Account : AuditableEntity<Guid>
{
    public Guid CompanyId { get; private set; }
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public AccountType Type { get; private set; }
    public AccountSubtype Subtype { get; private set; }
    public Guid? ParentId { get; private set; }
    public bool IsReconcilable { get; private set; }
    public bool AllowPosting { get; private set; }
    public bool IsActive { get; private set; }

    private Account() { }

    public Account(
        Guid companyId,
        string code,
        string name,
        AccountType type,
        AccountSubtype subtype = AccountSubtype.None,
        Guid? parentId = null,
        bool isReconcilable = false,
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
            isReconcilable,
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
        bool isReconcilable,
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
        IsReconcilable = isReconcilable;
        AllowPosting = allowPosting;
        IsActive = isActive;
        Touch();
    }
}
