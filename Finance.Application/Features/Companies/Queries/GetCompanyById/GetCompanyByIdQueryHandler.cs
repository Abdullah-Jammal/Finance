using Finance.Application.Contracts.Company;
using Finance.Application.Features.Companies.Queries.GetAllCompanies;
using MediatR;

namespace Finance.Application.Features.Companies.Queries.GetCompanyById;

public sealed class GetCompanyByIdQueryHandler(ICompanyService service) : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
{
    public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        return await service.GetByIdAsync(request.Id, cancellationToken);
    }
}
