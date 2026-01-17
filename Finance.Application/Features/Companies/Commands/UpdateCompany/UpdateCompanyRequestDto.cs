
namespace Finance.Application.Features.Companies.Commands.UpdateCompany;

public sealed class UpdateCompanyRequestDto
{
    public string Name { get; init; } = default!;
    public string Code { get; init; } = default!;
    public string BaseCurrencyCode { get; init; } = default!;
}
