using Finance.Domain.Entities.Auth;
using Finance.Domain.Entities.Company;
using Finance.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance.Infrastructure.Persistence.Configurations.Schemas.Auth;

public sealed class UserCompanyRoleConfiguration
    : IEntityTypeConfiguration<UserCompanyRole>
{
    public void Configure(EntityTypeBuilder<UserCompanyRole> builder)
    {
        const string schema = "auth";

        builder.ToTable("user_company_roles", schema);
        builder.HasKey(x => new { x.UserId, x.CompanyId, x.RoleId });

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder.HasOne<Company>()
            .WithMany()
            .HasForeignKey(x => x.CompanyId);

        builder.HasOne<IdentityRole<Guid>>()
            .WithMany()
            .HasForeignKey(x => x.RoleId);
    }
}
