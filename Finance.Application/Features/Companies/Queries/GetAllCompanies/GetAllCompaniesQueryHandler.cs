using Finance.Application.Common.Pagination;
using Finance.Application.Contracts.Company;
using MediatR;

namespace Finance.Application.Features.Companies.Queries.GetAllCompanies;

public sealed class GetAllCompaniesQueryHandler(ICompanyService service) : IRequestHandler<GetAllCompaniesQuery, PagedResult<CompanyDto>>
{
    public async Task<PagedResult<CompanyDto>> Handle(
        GetAllCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        return await service.GetAllAsync(
            request.PageNumber,
            request.PageSize,
            request.IsActive,
            request.Code,
            request.Search,
            request.SortBy,
            request.SortDirection,
            cancellationToken);
    }
}
