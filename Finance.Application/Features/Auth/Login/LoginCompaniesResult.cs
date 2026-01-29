namespace Finance.Application.Features.Auth.Login;

public sealed record LoginCompaniesResult
{
    public required Guid UserId { get; init; }
    public required string FullName { get; init; }
    public required IReadOnlyList<LoginCompanyDto> Companies { get; init; }
}
