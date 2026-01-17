using Finance.Application.Common.Exceptions;
using Finance.Application.Common.Pagination;
using Finance.Application.Common.Sorting;
using Finance.Application.Contracts.Company;
using Finance.Application.Features.Companies.Queries.GetAllCompanies;
using Finance.Domain.Entities.Company;
using Microsoft.EntityFrameworkCore;

namespace Finance.Infrastructure.Services.CompanyService;

public sealed class CompanyService(FinanceDbContext db) : ICompanyService
{
    public async Task<Guid> CreateAsync(
        string name,
        string code,
        string baseCurrencyCode,
        CancellationToken ct)
    {
        var exists = await db.Set<Company>()
            .AnyAsync(x => x.Code == code, ct);

        if (exists)
            throw new InvalidOperationException(
                $"Company with code '{code}' already exists.");

        var company = new Company(name, code, baseCurrencyCode);

        db.Add(company);
        await db.SaveChangesAsync(ct);

        return company.Id;
    }

    // Get all companies
    public async Task<PagedResult<CompanyDto>> GetAllAsync(
        int pageNumber,
        int pageSize,
        bool? isActive,
        string? code,
        string? search,
        CompanySortField sortBy,
        SortDirection sortDirection,
        CancellationToken ct)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

        IQueryable<Company> query = db.Set<Company>().AsNoTracking();

        if (isActive.HasValue)
            query = query.Where(x => x.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(code))
            query = query.Where(x => x.Code == code.ToUpper());

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(x =>
                EF.Functions.ILike(x.Name, $"%{search}%"));

        query = (sortBy, sortDirection) switch
        {
            (CompanySortField.CreatedAt, SortDirection.Asc)
                => query.OrderBy(x => x.CreatedAt),

            (CompanySortField.CreatedAt, SortDirection.Desc)
                => query.OrderByDescending(x => x.CreatedAt),

            _ => query.OrderBy(x => x.Name)
        };

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new CompanyDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                BaseCurrencyCode = x.BaseCurrencyCode,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)pageSize);

        return new PagedResult<CompanyDto>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    // Get company by id
    public async Task<CompanyDto> GetByIdAsync(
    Guid id,
    CancellationToken ct)
    {
        var company = await db.Set<Company>()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new CompanyDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                BaseCurrencyCode = x.BaseCurrencyCode,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync(ct);

        if (company is null)
            throw NotFoundException.ForEntity("Company", id);

        return company;
    }

    // Update company
    public async Task UpdateAsync(
    Guid id,
    string name,
    string code,
    string baseCurrencyCode,
    CancellationToken ct)
    {
        var company = await db.Set<Company>()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (company is null)
            throw NotFoundException.ForEntity("Company", id);

        company.UpdateDetails(
            name,
            code,
            baseCurrencyCode);
        await db.SaveChangesAsync(ct);
    }

    // Soft delete company
    public async Task SoftDeleteAsync(
    Guid id,
    CancellationToken ct)
    {
        var company = await db.Set<Company>()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (company is null)
            throw NotFoundException.ForEntity("Company", id);

        company.Deactivate();
        await db.SaveChangesAsync(ct);
    }

    // Restore company
    public async Task RestoreAsync(
    Guid id,
    CancellationToken ct)
    {
        var company = await db.Set<Company>()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (company is null)
            throw NotFoundException.ForEntity("Company", id);

        company.Activate();
        await db.SaveChangesAsync(ct);
    }
}
