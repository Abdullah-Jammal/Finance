using MediatR;

namespace Finance.Application.Features.User.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId)
    : IRequest<UserDetailsDto>;
