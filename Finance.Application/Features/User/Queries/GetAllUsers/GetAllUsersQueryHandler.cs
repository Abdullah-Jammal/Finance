using Finance.Application.Common.Pagination;
using Finance.Application.Contracts.User;
using Finance.Application.Features.User.Queries.GetAllUsers;
using MediatR;

public sealed class GetAllUsersQueryHandler(
    IUserManagementService repository)
    : IRequestHandler<GetAllUsersQuery, PagedResult<UserListItemDto>>
{
    public Task<PagedResult<UserListItemDto>> Handle(
        GetAllUsersQuery request,
        CancellationToken ct)
    {
        return repository.GetAllAsync(
            request.CompanyId,
            request.Search,
            request.SortBy,
            request.SortDirection,
            request.PageNumber,
            request.PageSize,
            ct);
    }
}
