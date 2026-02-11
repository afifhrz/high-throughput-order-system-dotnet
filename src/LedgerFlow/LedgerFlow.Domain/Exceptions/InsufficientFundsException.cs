namespace LedgerFlow.Domain.Exceptions;

public class InsufficientFundsException : Exception
{
    public Guid AccountId { get; }
    public InsufficientFundsException(Guid accountId)
        : base($"Insufficient funds in account with ID: {accountId}")
    {
        AccountId = accountId;
    }
}