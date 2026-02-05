using Finance.Domain.Enums;

namespace Finance.Application.Accounting.CoaTemplates;

public sealed record CoaAccountDefinition(
    string Code,
    string Name,
    AccountType Type,
    AccountSubtype Subtype,
    string? ParentCode,
    bool AllowPosting,
    bool IsActive = true);
