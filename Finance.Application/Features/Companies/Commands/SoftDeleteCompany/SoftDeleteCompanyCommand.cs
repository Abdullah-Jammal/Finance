using MediatR;

namespace Finance.Application.Features.Companies.Commands.SoftDeleteCompany;

public sealed record SoftDeleteCompanyCommand(Guid Id) : IRequest;