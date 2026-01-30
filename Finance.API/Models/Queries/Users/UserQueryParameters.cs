using Finance.Application.Common.Sorting;
using Finance.Application.Features.Users.Queries.GetAllUsers;

namespace Finance.API.Models.Queries.Users;

public sealed class UserQueryParameters
{
    public Guid? CompanyId { get; init; }
    public string? Search { get; init; }

    public UserSortField SortBy { get; init; } = UserSortField.FullName;

    public SortDirection SortDirection { get; init; } = SortDirection.Asc;

    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}
