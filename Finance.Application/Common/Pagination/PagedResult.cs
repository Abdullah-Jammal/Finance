
namespace Finance.Application.Common.Pagination;

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
}
