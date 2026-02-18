using LedgerFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LedgerFlow.Infrastructure.Seed;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        var host = builder.ConfigureAppConfiguration((ctx, cfg) =>
            {
                cfg.AddJsonFile("appsettings.json", optional: true);
                cfg.AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", optional: true);
            })
            .ConfigureServices((ctx, services) =>
            {
                var conn = ctx.Configuration.GetConnectionString("MySql")
                           ?? throw new InvalidOperationException("Connection string 'MySql' not found.");
                services.AddDbContext<LedgerFlowDbContext>(opts =>
                    opts.UseMySql(conn, new MySqlServerVersion(new Version(8, 0, 31))));
            })
            .Build();

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LedgerFlowDbContext>();

        try
        {
            // make sure MySql server is available
            Console.WriteLine("Checking database connectivity...");
            while (true)
            {
                try
                {
                    if (await db.Database.CanConnectAsync())
                    {
                        Console.WriteLine("Database connection successful.");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Database connection failed: {ex.Message}");
                }
                Console.WriteLine("Retrying in 5 seconds...");
                await Task.Delay(5000);
            }

            Console.WriteLine("Applying EF Core migrations...");
            await db.Database.MigrateAsync();
            Console.WriteLine("Migrations applied.");

            Console.WriteLine("Seeding initial data...");
            await AccountSeeder.SeedAsync(db);
            Console.WriteLine("Data seeding completed.");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Migration/seeding failed: {ex}");
            return 2;
        }
    }
}