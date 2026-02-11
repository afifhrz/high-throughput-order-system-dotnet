using LedgerFlow.Domain.Enums;

namespace LedgerFlow.Domain.Entities;

public class PaymentTransaction
{
    public Guid Id { get; private set; }
    public string IdempotencyKey { get; private set; } = null!;
    public TransactionStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private PaymentTransaction() { }

    public PaymentTransaction(string idempotencyKey)
    {
        Id = Guid.NewGuid();
        IdempotencyKey = idempotencyKey;
        Status = TransactionStatus.Completed;
        CreatedAt = DateTime.UtcNow;
    }
}
