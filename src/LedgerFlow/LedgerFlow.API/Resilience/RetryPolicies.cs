using Microsoft.EntityFrameworkCore;
using Polly;
using Serilog;

namespace LedgerFlow.API.Resilience;

public static class RetryPolicies
{
    public static AsyncPolicy CreateDbRetryPolicy()
    {
        return Policy
            .Handle<DbUpdateConcurrencyException>()
            .Or<DbUpdateException>(ex => IsTransient(ex))
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retry =>
                    TimeSpan.FromMilliseconds(50 * retry),
                onRetry: (exception, delay, retryCount, _) =>
                {
                    Log.Information(
                        $"CreateDbRetryPolicy.RetryPolicies: Retry {retryCount} due to {exception.GetType().Name}");
                });
    }

    private static bool IsTransient(DbUpdateException ex)
    {
        // Keep simple & explainable
        return ex.InnerException != null;
    }
}
