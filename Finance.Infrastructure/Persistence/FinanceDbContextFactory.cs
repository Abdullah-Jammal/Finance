using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Finance.Infrastructure.Persistence;

public sealed class FinanceDbContextFactory
    : IDesignTimeDbContextFactory<FinanceDbContext>
{
    public FinanceDbContext CreateDbContext(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddInfrastructure(context.Configuration);
            })
            .Build();

        var scope = host.Services.CreateScope();

        return scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
    }
}
