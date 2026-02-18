namespace LedgerFlow.API.Payments;

public record PaymentRequest(
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount
);
