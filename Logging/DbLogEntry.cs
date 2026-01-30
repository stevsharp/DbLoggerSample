
namespace DbLoggerSample.Logging;

public class DbLogEntry
{
    public DateTime Ts { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? TraceId { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Exception { get; set; }
    public string? Scopes { get; set; }
}
