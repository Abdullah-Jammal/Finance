namespace Finance.API.Models.Queries.Common;

public sealed class PagingParameters
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
