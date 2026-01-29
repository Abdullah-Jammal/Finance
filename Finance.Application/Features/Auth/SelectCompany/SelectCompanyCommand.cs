using MediatR;

namespace Finance.Application.Features.Auth.SelectCompany;

public sealed record SelectCompanyCommand(
    Guid UserId,
    Guid CompanyId
) : IRequest<LoginResult>;
