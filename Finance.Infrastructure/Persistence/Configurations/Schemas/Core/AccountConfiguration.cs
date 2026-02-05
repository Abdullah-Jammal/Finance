using Finance.Domain.Entities.Accounting;
using Finance.Domain.Entities.Company;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance.Infrastructure.Persistence.Configurations.Schemas.Core;

public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        const string schema = "core";
        builder.ToTable("accounts", schema);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CompanyId).IsRequired();
        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(x => x.Type)
            .IsRequired();
        builder.Property(x => x.Subtype)
            .HasMaxLength(50);
        builder.Property(x => x.IsReconcilable).IsRequired();
        builder.Property(x => x.AllowPosting).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();

        builder.HasIndex(x => new { x.CompanyId, x.Code }).IsUnique();
        builder.HasIndex(x => x.ParentId);

        builder.HasOne<Company>()
            .WithMany()
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Account>()
            .WithMany()
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
