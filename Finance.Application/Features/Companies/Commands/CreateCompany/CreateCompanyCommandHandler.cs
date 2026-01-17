using Finance.Application.Contracts.Company;
using MediatR;

namespace Finance.Application.Features.Companies.Commands.CreateCompany;

public sealed class CreateCompanyCommandHandler(ICompanyService service)
    : IRequestHandler<CreateCompanyCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateCompanyCommand request,
        CancellationToken cancellationToken)
    {
        return await service.CreateAsync(
            request.Name,
            request.Code,
            request.BaseCurrencyCode,
            cancellationToken);
    }
}
