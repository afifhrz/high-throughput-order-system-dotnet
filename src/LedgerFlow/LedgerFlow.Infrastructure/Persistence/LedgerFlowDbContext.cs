using LedgerFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LedgerFlow.Infrastructure.Persistence;

public class LedgerFlowDbContext : DbContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<LedgerEntry> LedgerEntries => Set<LedgerEntry>();
    public DbSet<PaymentTransaction> Transactions => Set<PaymentTransaction>();

    public LedgerFlowDbContext(DbContextOptions<LedgerFlowDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LedgerFlowDbContext).Assembly);
    }
}
