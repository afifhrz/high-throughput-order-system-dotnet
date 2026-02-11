namespace LedgerFlow.Domain.Entities;

public class LedgerEntry
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid TransactionId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private LedgerEntry() { }

    public LedgerEntry(Guid accountId, Guid transactionId, decimal amount)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        TransactionId = transactionId;
        Amount = amount;
        CreatedAt = DateTime.UtcNow;
    }
}
