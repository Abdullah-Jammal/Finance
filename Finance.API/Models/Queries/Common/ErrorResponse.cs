namespace Finance.API.Models.Queries.Common;

public sealed class ErrorResponse
{
    public int StatusCode { get; init; }
    public string Message { get; init; } = default!;
    public string? ErrorCode { get; init; }
    public object? Errors { get; init; }
}
