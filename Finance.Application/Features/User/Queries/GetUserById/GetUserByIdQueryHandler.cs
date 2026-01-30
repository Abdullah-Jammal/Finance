using Finance.Application.Contracts.User;
using MediatR;

namespace Finance.Application.Features.User.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(
    IUserManagementService repository)
    : IRequestHandler<GetUserByIdQuery, UserDetailsDto>
{
    public Task<UserDetailsDto> Handle(
        GetUserByIdQuery request,
        CancellationToken ct)
    {
        return repository.GetByIdAsync(request.UserId, ct);
    }
}