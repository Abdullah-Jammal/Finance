using MediatR;

namespace Finance.Application.Features.Companies.Commands.CreateCompany;

public sealed record CreateCompanyCommand
(
    string Name,
    string Code,
    string BaseCurrencyCode
) : IRequest<Guid>;
