using Finance.Application.Accounting.Services;
using Finance.Application.Contracts.Company;
using MediatR;

namespace Finance.Application.Features.Companies.Commands.CreateCompany;

public sealed class CreateCompanyCommandHandler(
    ICompanyService service,
    ICoaSeeder coaSeeder)
    : IRequestHandler<CreateCompanyCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var companyId = await service.CreateAsync(
            request.Name,
            request.Code,
            request.BaseCurrencyCode,
            cancellationToken);

        await coaSeeder.SeedAsync(companyId, cancellationToken);

        return companyId;
    }
}
