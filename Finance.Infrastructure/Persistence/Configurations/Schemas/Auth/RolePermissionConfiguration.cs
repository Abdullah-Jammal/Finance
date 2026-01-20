using Finance.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance.Infrastructure.Persistence.Configurations.Schemas.Auth;

public sealed class RolePermissionConfiguration
    : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        const string schema = "auth";

        builder.ToTable("role_permissions", schema);
        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        builder.HasOne<IdentityRole<Guid>>()
            .WithMany()
            .HasForeignKey(x => x.RoleId);

        builder.HasOne<Permission>()
            .WithMany()
            .HasForeignKey(x => x.PermissionId);
    }
}
