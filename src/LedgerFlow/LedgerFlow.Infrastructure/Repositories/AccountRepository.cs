using LedgerFlow.Domain.Entities;
using LedgerFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LedgerFlow.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly LedgerFlowDbContext _db;

    public AccountRepository(LedgerFlowDbContext db)
    {
        _db = db;
    }

    public Task<Account?> GetAsync(Guid id)
    {
        return _db.Accounts.SingleOrDefaultAsync(a => a.Id == id);
    }

    public Task UpdateAsync(Account account)
    {
        _db.Accounts.Update(account);
        return Task.CompletedTask;
    }
}
