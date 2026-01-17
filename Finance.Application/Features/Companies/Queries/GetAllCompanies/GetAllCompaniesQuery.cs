using Finance.Application.Common.Pagination;
using Finance.Application.Common.Sorting;
using MediatR;

namespace Finance.Application.Features.Companies.Queries.GetAllCompanies;

public sealed record GetAllCompaniesQuery(
    int PageNumber = 1,
    int PageSize = 10,
    bool? IsActive = null,
    string? Code = null,
    string? Search = null,
    CompanySortField SortBy = CompanySortField.Name,
    SortDirection SortDirection = SortDirection.Asc
) : IRequest<PagedResult<CompanyDto>>;
