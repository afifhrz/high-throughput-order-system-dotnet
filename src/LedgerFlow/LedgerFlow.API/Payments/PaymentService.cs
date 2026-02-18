using LedgerFlow.API.Resilience;
using LedgerFlow.Domain.Entities;
using LedgerFlow.Infrastructure.Idempotency;
using LedgerFlow.Infrastructure.Persistence;
using Polly;

namespace LedgerFlow.API.Payments;

public class PaymentService
{
    private readonly LedgerFlowDbContext _db;
    private readonly IIdempotencyService _idempotency;
    private readonly AsyncPolicy _retryPolicy;

    public PaymentService(
        LedgerFlowDbContext db, 
        IIdempotencyService idempotency,
        ILogger<PaymentService> logger)
    {
        _db = db;
        _idempotency = idempotency;
        _retryPolicy = RetryPolicies.CreateDbRetryPolicy();
    }

    public async Task<Guid> ProcessAsync(
        PaymentRequest request,
        string idempotencyKey)
    {
        var cached = await _idempotency.GetAsync(idempotencyKey);
        if (cached != null)
            return Guid.Parse(cached);

        Guid transactionId = Guid.Empty;

        await _retryPolicy.ExecuteAsync(async () =>
        {
            using var tx = await _db.Database.BeginTransactionAsync();

            var from = await _db.Accounts.FindAsync(request.FromAccountId);
            var to = await _db.Accounts.FindAsync(request.ToAccountId);

            from!.Debit(request.Amount);
            to!.Credit(request.Amount);

            var transaction = new PaymentTransaction(idempotencyKey);
            _db.Transactions.Add(transaction);

            _db.LedgerEntries.Add(
                new LedgerEntry(from.Id, transaction.Id, -request.Amount));
            _db.LedgerEntries.Add(
                new LedgerEntry(to.Id, transaction.Id, request.Amount));

            await _db.SaveChangesAsync();
            await tx.CommitAsync();

            transactionId = transaction.Id;
        });

        await _idempotency.SetAsync(
            idempotencyKey,
            transactionId.ToString(),
            TimeSpan.FromHours(24));

        return transactionId;
    }
}
