using LedgerFlow.Domain.Entities;

namespace LedgerFlow.Infrastructure.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetAsync(Guid id);
    Task UpdateAsync(Account account);
}
