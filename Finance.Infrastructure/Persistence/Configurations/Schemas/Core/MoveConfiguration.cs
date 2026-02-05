using Finance.Domain.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finance.Infrastructure.Persistence.Configurations.Schemas.Core;

public sealed class MoveConfiguration : IEntityTypeConfiguration<Move>
{
    public void Configure(EntityTypeBuilder<Move> builder)
    {
        const string schema = "core";
        builder.ToTable("moves", schema);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CompanyId).IsRequired();
        builder.Property(x => x.Date).IsRequired();
        builder.Property(x => x.IsPosted).IsRequired();

        builder.OwnsMany(x => x.Lines, line =>
        {
            line.ToTable("move_lines", schema);
            line.WithOwner().HasForeignKey("MoveId");
            line.HasKey("Id");
            line.Property<Guid>("Id");

            line.Property(x => x.AccountId).IsRequired();
            line.Property(x => x.Debit)
                .HasPrecision(18, 2)
                .IsRequired();
            line.Property(x => x.Credit)
                .HasPrecision(18, 2)
                .IsRequired();
            line.Property(x => x.PartnerId);

            line.HasOne(x => x.Account)
                .WithMany()
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Navigation(x => x.Lines)
            .HasField("_lines")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
