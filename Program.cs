using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using OrderAPI.Repositories;
using OrderAPI.Services;

// Load environment variables from .env file
LoadEnvironmentVariables();

var builder = WebApplication.CreateBuilder(args);

// Build connection string from environment variables
var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "OrderDb";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "postgres";

var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

// Add services to the container
builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// Add controllers
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Order API",
        Version = "v1",
        Description = "A simple API for managing orders with PostgreSQL",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Order API Support"
        }
    });

    // Include XML comments for better documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Add CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Apply database migrations on startup (optional)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    dbContext.Database.Migrate();
}

app.Run();

// Loads environment variables from .env file
void LoadEnvironmentVariables()
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
