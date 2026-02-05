using Finance.Domain.Enums;
using MediatR;

namespace Finance.Application.Features.Accounts.Commands.CreateAccount;

public sealed record CreateAccountCommand(
    Guid CompanyId,
    string Code,
    string Name,
    AccountType Type,
    AccountSubtype Subtype,
    Guid? ParentId,
    bool AllowPosting,
    bool IsActive) : IRequest<Guid>;
