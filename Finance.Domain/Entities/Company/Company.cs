using Finance.Domain.Abstractions;

namespace Finance.Domain.Entities.Company;

public class Company : AuditableEntity<Guid>
{
    public string Name { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public string BaseCurrencyCode { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    public DateTime? DeletedAt { get; private set; }

    private Company() { }

    public Company(string name, string code, string baseCurrencyCode)
    {
        UpdateDetails(name, code, baseCurrencyCode);
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
        DeletedAt = DateTime.UtcNow;
        Touch();
    }

    public void UpdateDetails(
    string name,
    string code,
    string baseCurrencyCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Company name is required");

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Company code is required");

        if (string.IsNullOrWhiteSpace(baseCurrencyCode))
            throw new ArgumentException("Base currency is required");

        code = code.Trim().ToUpperInvariant();
        baseCurrencyCode = baseCurrencyCode.Trim().ToUpperInvariant();

        if (baseCurrencyCode.Length != 3)
            throw new ArgumentException("Base currency code must be exactly 3 characters");

        Name = name.Trim();
        BaseCurrencyCode = baseCurrencyCode;
        Code = code;

        Touch();
    }
    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;
        DeletedAt = null;
        Touch();
    }
}
