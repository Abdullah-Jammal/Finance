
namespace Finance.Application.Features.Companies.Queries.GetAllCompanies;

public sealed class CompanyDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string Code { get; init; } = default!;
    public string BaseCurrencyCode { get; init; } = default!;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
