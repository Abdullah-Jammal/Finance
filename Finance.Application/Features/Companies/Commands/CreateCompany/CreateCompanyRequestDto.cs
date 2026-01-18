
namespace Finance.Application.Features.Companies.Commands.CreateCompany;

public sealed class CreateCompanyRequestDto
{
    public string Name { get; init; } = default!;
    public string Code { get; init; } = default!;
    public string BaseCurrencyCode { get; init; } = default!;
}
