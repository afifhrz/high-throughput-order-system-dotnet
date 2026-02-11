namespace LedgerFlow.Infrastructure.Idempotency;

public interface IIdempotencyService
{
    Task<string?> GetAsync(string key);
    Task SetAsync(string key, string value, TimeSpan ttl);
}
