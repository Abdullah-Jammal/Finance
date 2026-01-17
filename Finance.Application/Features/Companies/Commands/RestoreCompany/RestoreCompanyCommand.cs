using MediatR;

namespace Finance.Application.Features.Companies.Commands.RestoreCompany;

public sealed record RestoreCompanyCommand(Guid Id) : IRequest;
