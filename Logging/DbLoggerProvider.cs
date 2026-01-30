
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Threading.Channels;

namespace DbLoggerSample.Logging;

public class DbLoggerProvider : ILoggerProvider, ISupportExternalScope
{
    private readonly DbLogOptions _options;
    private readonly Channel<DbLogEntry> _channel;
    private IExternalScopeProvider? _scopeProvider;
    private readonly CancellationTokenSource _cts = new();

    public DbLoggerProvider(IOptions<DbLogOptions> options)
    {
        _options = options.Value;
        _channel = Channel.CreateBounded<DbLogEntry>(_options.MaxQueue);
        Task.Run(ProcessQueueAsync);
    }

    public ILogger CreateLogger(string categoryName) => new DbLogger(categoryName, _options, _channel, _scopeProvider);

    public void SetScopeProvider(IExternalScopeProvider scopeProvider) => _scopeProvider = scopeProvider;

    private async Task ProcessQueueAsync()
    {
        while (!_cts.IsCancellationRequested)
        {
            var entry = await _channel.Reader.ReadAsync(_cts.Token);

            const string sql = @"INSERT INTO dbo.AppLogs (Ts, Level, Category, TraceId, Message, Exception, Scopes)
                                VALUES (@Ts, @Level, @Category, @TraceId, @Message, @Exception, @Scopes)";

            using var conn = new SqlConnection(_options.ConnectionString);
            await conn.ExecuteAsync(sql, entry);
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _channel.Writer.TryComplete();
    }
}
