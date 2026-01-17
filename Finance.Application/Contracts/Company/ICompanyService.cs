using Finance.Application.Common.Pagination;
using Finance.Application.Common.Sorting;
using Finance.Application.Features.Companies.Queries.GetAllCompanies;

namespace Finance.Application.Contracts.Company;

public interface ICompanyService
{
    Task<Guid> CreateAsync(
        string name,
        string code,
        string baseCurrencyCode,
        CancellationToken ct);
    Task<PagedResult<CompanyDto>> GetAllAsync(
        int pageNumber,
        int pageSize,
        bool? isActive,
        string? code,
        string? search,
        CompanySortField sortBy,
        SortDirection sortDirection,
        CancellationToken ct);
    Task<CompanyDto> GetByIdAsync(
        Guid id,
        CancellationToken ct);
    Task UpdateAsync(
    Guid id,
    string name,
    string code,
    string baseCurrencyCode,
    CancellationToken ct);
    Task SoftDeleteAsync(
        Guid id,
        CancellationToken ct);
    Task RestoreAsync(
    Guid id,
    CancellationToken ct);
}
