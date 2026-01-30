namespace DbLoggerSample.Logging;

public static class DbLoggerExtensions
{
    public static ILoggingBuilder AddDatabaseLogger(this ILoggingBuilder builder, Action<DbLogOptions> configure)
    {
        builder.Services.Configure(configure);
        builder.Services.AddSingleton<ILoggerProvider, DbLoggerProvider>();
        return builder;
    }
}
