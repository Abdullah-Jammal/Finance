using MediatR;

namespace Finance.Application.Features.Companies.Commands.UpdateCompany;

public sealed record UpdateCompanyCommand(
    Guid Id,
    string Name,
    string Code,
    string BaseCurrencyCode
) : IRequest;
