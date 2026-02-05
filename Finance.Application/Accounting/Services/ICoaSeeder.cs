namespace Finance.Application.Accounting.Services;

public interface ICoaSeeder
{
    Task SeedAsync(Guid companyId, CancellationToken ct);
}
