using Finance.Application.Contracts.Company;
using MediatR;

namespace Finance.Application.Features.Companies.Commands.SoftDeleteCompany;

public sealed class SoftDeleteCompanyCommandHandler(ICompanyService service)
    : IRequestHandler<SoftDeleteCompanyCommand>
{
    public async Task Handle(
        SoftDeleteCompanyCommand request,
        CancellationToken cancellationToken)
    {
        await service.SoftDeleteAsync(
            request.Id,
            cancellationToken);
    }
}
