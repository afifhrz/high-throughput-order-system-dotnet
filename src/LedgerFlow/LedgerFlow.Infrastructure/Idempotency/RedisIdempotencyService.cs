using StackExchange.Redis;

namespace LedgerFlow.Infrastructure.Idempotency;

public class RedisIdempotencyService : IIdempotencyService
{
    private readonly IDatabase _db;

    public RedisIdempotencyService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task<string?> GetAsync(string key)
    {
        var value = await _db.StringGetAsync(key);
        return value.HasValue ? value.ToString() : null;
    }

    public Task SetAsync(string key, string value, TimeSpan ttl)
    {
        return _db.StringSetAsync(key, value, ttl);
    }
}
