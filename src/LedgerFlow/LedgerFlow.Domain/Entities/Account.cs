using LedgerFlow.Domain.Exceptions;

namespace LedgerFlow.Domain.Entities;

public class Account
{
    public Guid Id { get; private set; }
    public string Currency { get; private set; } = null!;
    public decimal Balance { get; private set; }

    public byte[] Version { get; private set; } = null!; 

    private Account() { }

    public Account(Guid id, string currency, decimal initialBalance)
    {
        Id = id;
        Currency = currency;
        Balance = initialBalance;
    }

    public void Debit(decimal amount)
    {
        if (Balance < amount)
            throw new InsufficientFundsException(Id);

        Balance -= amount;
    }

    public void Credit(decimal amount)
    {
        Balance += amount;
    }
}
