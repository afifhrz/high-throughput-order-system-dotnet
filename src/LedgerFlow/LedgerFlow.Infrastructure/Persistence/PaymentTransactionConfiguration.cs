using LedgerFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LedgerFlow.Infrastructure.Persistence;

public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.IdempotencyKey)
            .IsRequired();

        builder.HasIndex(t => t.IdempotencyKey)
            .IsUnique();

        builder.Property(t => t.Status)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .IsRequired();
    }
}
