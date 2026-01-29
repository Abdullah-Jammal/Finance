using Finance.Application.Contracts.User;
using MediatR;

namespace Finance.Application.Features.User.Commands.AssignCompany;

public sealed class AssignUserToCompanyCommandHandler(
    IUserManagementService service)
    : IRequestHandler<AssignUserToCompanyCommand>
{
    public async Task Handle(
        AssignUserToCompanyCommand request,
        CancellationToken ct)
    {
        await service.AssignUserToCompanyAsync(
            request.UserId,
            request.CompanyId,
            ct);
    }
}
