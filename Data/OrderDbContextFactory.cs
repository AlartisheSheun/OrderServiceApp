using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrderAPI.Data;

// Design-time factory for Entity Framework Core tools (for migrations)
public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
{
    public OrderDbContext CreateDbContext(string[] args)
    {
        // Load environment variables from .env file
        LoadEnvironmentVariables();

        // Build connection string from environment variables
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
        var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "OrderDb";
        var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "postgres";

        var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};";

        var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new OrderDbContext(optionsBuilder.Options);
    }

    private static void LoadEnvironmentVariables()
    {
        // Try to find .env file by walking up from current directory
        string? envPath = null;
        var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (currentDir != null)
        {
            var potentialPath = Path.Combine(currentDir.FullName, ".env");
            if (File.Exists(potentialPath))
            {
                envPath = potentialPath;
                break;
            }
            currentDir = currentDir.Parent;
        }

        if (string.IsNullOrEmpty(envPath))
            return;

        foreach (var line in File.ReadAllLines(envPath))
        {
            // Skip empty lines and comments
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            var parts = line.Split('=', 2);
            if (parts.Length != 2)
                continue;

            var key = parts[0].Trim();
            var value = parts[1].Trim();

            // Only set if not already set in environment
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)))
            {
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }
}
