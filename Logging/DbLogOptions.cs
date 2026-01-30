
namespace DbLoggerSample.Logging;

public class DbLogOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public LogLevel MinLevel { get; set; } = LogLevel.Information;
    public int MaxQueue { get; set; } = 5000;
}
