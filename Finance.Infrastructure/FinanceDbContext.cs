using Finance.Infrastructure.Identity;
using Finance.Infrastructure.Persistence.Configurations.Schemas.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finance.Infrastructure;

public sealed class FinanceDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
        : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("core");
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(FinanceDbContext).Assembly);

        AuthSchemaConfiguration.Configure(builder);
    }
}
