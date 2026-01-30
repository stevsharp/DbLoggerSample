
using DbLoggerSample.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Logging.AddDatabaseLogger(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                           ?? "Server=localhost;Database=LogsDb;Trusted_Connection=True;TrustServerCertificate=True;";
    options.MinLevel = LogLevel.Information;
    options.MaxQueue = 5000;
});

var app = builder.Build();

app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation("Hello from DB Logger!");
    return Results.Ok("Logged!");
});

app.Run();
