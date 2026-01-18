using Finance.Application.Common.Sorting;
using Finance.Application.Features.Companies.Queries.GetAllCompanies;

namespace Finance.API.Models.Queries.Companies;

public sealed class CompanyQueryParameters
{
    public bool? IsActive { get; init; }
    public string? Code { get; init; }
    public string? Search { get; init; }
    public CompanySortField SortBy { get; init; } = CompanySortField.Name;
    public SortDirection SortDirection { get; init; } = SortDirection.Asc;
}
