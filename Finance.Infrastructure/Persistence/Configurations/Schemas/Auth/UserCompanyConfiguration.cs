using Finance.Domain.Entities.Auth;
using Finance.Domain.Entities.Company;
using Finance.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance.Infrastructure.Persistence.Configurations.Schemas.Auth;

public sealed class UserCompanyConfiguration
    : IEntityTypeConfiguration<UserCompany>
{
    public void Configure(EntityTypeBuilder<UserCompany> builder)
    {
        const string schema = "auth";

        builder.ToTable("user_companies", schema);
        builder.HasKey(x => new { x.UserId, x.CompanyId });

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder.HasOne<Company>()
            .WithMany()
            .HasForeignKey(x => x.CompanyId);
    }
}
