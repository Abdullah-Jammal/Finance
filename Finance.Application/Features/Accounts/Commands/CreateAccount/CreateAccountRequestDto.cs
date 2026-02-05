using Finance.Domain.Enums;

namespace Finance.Application.Features.Accounts.Commands.CreateAccount;

public sealed class CreateAccountRequestDto
{
    public Guid CompanyId { get; init; }
    public string Code { get; init; } = default!;
    public string Name { get; init; } = default!;
    public AccountType Type { get; init; }
    public string? Subtype { get; init; }
    public Guid? ParentId { get; init; }
    public bool IsReconcilable { get; init; } = false;
    public bool AllowPosting { get; init; } = true;
    public bool IsActive { get; init; } = true;
}
