using System.Diagnostics;
using System.Text.Json;
using System.Threading.Channels;

namespace DbLoggerSample.Logging;

public class DbLogger(string category, DbLogOptions options, Channel<DbLogEntry> channel, IExternalScopeProvider? scopeProvider) : ILogger
{
    private readonly string _category = category;
    private readonly DbLogOptions _options = options;
    private readonly Channel<DbLogEntry> _channel = channel;
    private readonly IExternalScopeProvider? _scopeProvider = scopeProvider;

    public IDisposable BeginScope<TState>(TState state) =>
        _scopeProvider?.Push(state) ?? NullScope.Instance;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _options.MinLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) 
            return;

        var traceId = Activity.Current?.TraceId.ToString();

        var scopes = new List<object?>();

        _scopeProvider?.ForEachScope((s, list) => list.Add(s), scopes);

        var entry = new DbLogEntry
        {
            Ts = DateTime.UtcNow,
            Level = logLevel.ToString(),
            Category = _category,
            TraceId = traceId,
            Message = formatter(state, exception),
            Exception = exception?.ToString(),
            Scopes = scopes.Count > 0 ? JsonSerializer.Serialize(scopes) : null
        };

        _channel.Writer.TryWrite(entry);
    }

    private class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose() { }
    }
}
