using Finance.Application.Features.Auth.Login;

public sealed record LoginCompaniesResult
{
    public Guid UserId { get; init; }
    public string FullName { get; init; } = default!;
    public string TempToken { get; init; } = default!;
    public IReadOnlyList<LoginCompanyDto> Companies { get; init; } = [];
}
