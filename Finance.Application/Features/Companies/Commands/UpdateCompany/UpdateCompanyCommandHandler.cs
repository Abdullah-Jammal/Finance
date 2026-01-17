using Finance.Application.Contracts.Company;
using MediatR;

namespace Finance.Application.Features.Companies.Commands.UpdateCompany;

public sealed class UpdateCompanyCommandHandler(ICompanyService service) : IRequestHandler<UpdateCompanyCommand>
{
    public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        await service.UpdateAsync(
            request.Id,
            request.Name,
            request.Code,
            request.BaseCurrencyCode,
            cancellationToken);
    }
}
