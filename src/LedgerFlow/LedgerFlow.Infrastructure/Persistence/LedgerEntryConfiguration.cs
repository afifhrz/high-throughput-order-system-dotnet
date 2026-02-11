using LedgerFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerFlow.Infrastructure.Persistence;

public class LedgerEntryConfiguration : IEntityTypeConfiguration<LedgerEntry>
{
    public void Configure(EntityTypeBuilder<LedgerEntry> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(l => l.CreatedAt)
            .IsRequired();

        builder.HasIndex(l => l.TransactionId);
        builder.HasIndex(l => l.AccountId);
    }
}
