namespace Finance.Application.Features.Auth.Login;

public sealed record LoginCompanyDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
