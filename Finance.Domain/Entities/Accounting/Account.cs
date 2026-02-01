using Finance.Domain.Abstractions;

namespace Finance.Domain.Entities.Accounting;

public class Account : AuditableEntity<Guid>
{
    public Guid CompanyId { get; private set; }
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string Type { get; private set; } = default!;
    public string? Subtype { get; private set; }
    public Guid? ParentId { get; private set; }
    public bool IsReconcilable { get; private set; }
    public bool AllowPosting { get; private set; }
    public bool IsActive { get; private set; }

    private Account() { }

    public Account(
        Guid companyId,
        string code,
        string name,
        string type,
        string? subtype = null,
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
        string type,
        string? subtype,
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

        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Account type is required");

        CompanyId = companyId;
        Code = code.Trim();
        Name = name.Trim();
        Type = type.Trim();
        Subtype = string.IsNullOrWhiteSpace(subtype)
            ? null
            : subtype.Trim();
        ParentId = parentId;
        IsReconcilable = isReconcilable;
        AllowPosting = allowPosting;
        IsActive = isActive;
        Touch();
    }
}
