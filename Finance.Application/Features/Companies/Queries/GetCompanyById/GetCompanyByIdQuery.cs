using Finance.Application.Features.Companies.Queries.GetAllCompanies;
using MediatR;

namespace Finance.Application.Features.Companies.Queries.GetCompanyById;

public sealed record GetCompanyByIdQuery(Guid Id) : IRequest<CompanyDto>;
