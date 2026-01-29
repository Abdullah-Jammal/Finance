using MediatR;

namespace Finance.Application.Features.User.Commands.AssignCompany;

public sealed record AssignUserToCompanyCommand(
    Guid UserId,
    Guid CompanyId
) : IRequest;
