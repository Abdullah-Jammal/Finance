using Finance.Application.Contracts.Company;
using MediatR;

namespace Finance.Application.Features.Companies.Commands.RestoreCompany;

public sealed class RestoreCompanyCommandHandler(ICompanyService service)
    : IRequestHandler<RestoreCompanyCommand>
{
    public async Task Handle(
        RestoreCompanyCommand request,
        CancellationToken cancellationToken)
    {
        await service.RestoreAsync(
            request.Id,
            cancellationToken);
    }
}
