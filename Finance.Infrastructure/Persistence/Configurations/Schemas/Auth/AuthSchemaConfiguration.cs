using Finance.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Finance.Infrastructure.Persistence.Configurations.Schemas.Auth;

public static class AuthSchemaConfiguration
{
    private const string Schema = "auth";

    public static void Configure(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>().ToTable("users", Schema);
        builder.Entity<IdentityRole<Guid>>().ToTable("roles", Schema);
        builder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles", Schema);
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims", Schema);
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims", Schema);
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins", Schema);
        builder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens", Schema);
    }
}
