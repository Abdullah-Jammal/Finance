using Finance.Domain.Entities.Company;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance.Infrastructure.Persistence.Configurations.Schemas.Core;

public sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        const string schema = "core";
        builder.ToTable("companies", schema);

        builder.HasKey(x => x.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.BaseCurrencyCode)
            .IsRequired()
            .HasMaxLength(3);
        builder.Property(c => c.IsActive).IsRequired();
    }
}
