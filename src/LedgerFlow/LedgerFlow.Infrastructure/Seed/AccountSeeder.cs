using LedgerFlow.Domain.Entities;
using LedgerFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LedgerFlow.Infrastructure.Seed;

public static class AccountSeeder
{
    public static async Task SeedAsync(LedgerFlowDbContext db)
    {
        if (await db.Accounts.AnyAsync())
            return; // Idempotent: already seeded

        var accounts = new List<Account>
        {
            new(
                id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                currency: "USD",
                initialBalance: 1_000m
            ),
            new(
                id: Guid.Parse("22222222-2222-2222-2222-222222222222"),
                currency: "USD",
                initialBalance: 500m
            ),
            new(
                id: Guid.Parse("33333333-3333-3333-3333-333333333333"),
                currency: "USD",
                initialBalance: 10_000m
            )
        };

        db.Accounts.AddRange(accounts);
        await db.SaveChangesAsync();
    }
}
