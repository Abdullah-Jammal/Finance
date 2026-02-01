using MediatR;

namespace Finance.Application.Features.Accounts.Commands.CreateAccount;

public sealed record CreateAccountCommand(
    Guid CompanyId,
    string Code,
    string Name,
    string Type,
    string? Subtype,
    Guid? ParentId,
    bool IsReconcilable,
    bool AllowPosting,
    bool IsActive) : IRequest<Guid>;
