using Finance.Application.Common.Pagination;
using Finance.Application.Common.Sorting;
using Finance.Application.Features.Users.Queries.GetAllUsers;
using MediatR;

namespace Finance.Application.Features.User.Queries.GetAllUsers;

public sealed record GetAllUsersQuery(
    Guid? CompanyId,
    string? Search,
    UserSortField SortBy,
    SortDirection SortDirection,
    int PageNumber,
    int PageSize
) : IRequest<PagedResult<UserListItemDto>>;
