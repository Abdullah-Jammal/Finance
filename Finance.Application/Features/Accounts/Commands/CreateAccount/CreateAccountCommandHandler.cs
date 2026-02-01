using Finance.Application.Contracts.Account;
using MediatR;

namespace Finance.Application.Features.Accounts.Commands.CreateAccount;

public sealed class CreateAccountCommandHandler(IAccountService service)
    : IRequestHandler<CreateAccountCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateAccountCommand request,
        CancellationToken cancellationToken)
    {
        return await service.CreateAsync(
            request.CompanyId,
            request.Code,
            request.Name,
            request.Type,
            request.Subtype,
            request.ParentId,
            request.IsReconcilable,
            request.AllowPosting,
            request.IsActive,
            cancellationToken);
    }
}
