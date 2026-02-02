Read the full article at :  

https://coderlegion.com/10763/asp-net-core-custom-logger-save-logs-to-database-sql

https://dev.to/stevsharp/aspnet-core-custom-logger-save-logs-to-database-sql-3ne4

# DbLoggerSample

Simple ASP.NET Core (.NET 9) sample that logs to SQL Server using a custom logger with Channels.

## Quick start
1. Create table:
```sql
CREATE TABLE dbo.AppLogs(
  LogId BIGINT IDENTITY PRIMARY KEY,
  Ts DATETIME2(3) NOT NULL,
  Level NVARCHAR(20) NOT NULL,
  Category NVARCHAR(200) NOT NULL,
  TraceId NVARCHAR(64) NULL,
  Message NVARCHAR(MAX) NOT NULL,
  Exception NVARCHAR(MAX) NULL,
  Scopes NVARCHAR(MAX) NULL
);
```
2. Restore & run:
```bash
 dotnet restore
 dotnet run
```
3. Test in browser: `http://localhost:5000/` (or https port shown in console)

## Notes
- Configure connection string in `appsettings.json`.
- Table name is `dbo.AppLogs`.
