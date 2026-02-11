using LedgerFlow.API.Payments;
using LedgerFlow.Infrastructure.Idempotency;
using LedgerFlow.Infrastructure.Persistence;
using LedgerFlow.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StackExchange.Redis;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Host.UseSerilog();

        builder.Services.AddOpenApi();
        builder.Services.AddControllers();

        var mySqlConn = builder.Configuration.GetConnectionString("MySql");
        if (string.IsNullOrWhiteSpace(mySqlConn))
        {
            throw new InvalidOperationException("MySql connection string is not configured.");
        }
        builder.Services.AddDbContext<LedgerFlowDbContext>(options =>
            options.UseMySql(mySqlConn, new MySqlServerVersion(new Version(8, 0, 31))));

        var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
        if (string.IsNullOrWhiteSpace(redisConnectionString))
        {
            throw new InvalidOperationException("Redis connection string is not configured.");
        }
        builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var mux = ConnectionMultiplexer.Connect(redisConnectionString);
            return mux;
        });

        builder.Services.AddScoped<IIdempotencyService, RedisIdempotencyService>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<PaymentService>();

        var app = builder.Build();

        // Validate that we can connect to the database at startup (retry with detailed error logging)
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            while (true)
            {
                try
                {
                    var db = services.GetRequiredService<LedgerFlowDbContext>();
                    if (await db.Database.CanConnectAsync())
                    {
                        Log.Information("Database connection successful.");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to connect to the database. Retrying in 5 seconds...");
                }
                await Task.Delay(5000);
            }
            var redis = services.GetRequiredService<IConnectionMultiplexer>();
            while (true)
            {
                try
                {
                    var db = redis.GetDatabase();
                    var pong = await db.PingAsync();
                    Log.Information("Redis connection successful. Ping: {Ping} ms", pong.TotalMilliseconds);
                    break;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to connect to Redis. Retrying in 5 seconds...");
                }
                await Task.Delay(5000);
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        // Enable Serilog request logging
        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.MapControllers();

        try
        {
            Log.Information("Starting web host");
            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
