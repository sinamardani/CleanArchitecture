using Application.Commons.Interfaces;
using Application.Commons.Interfaces.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using ILogger = Serilog.ILogger;

namespace Infrastructure.Services;

public class LogService : ILogService, IDisposable
{
    private readonly ILogger _dbLogger;
    private readonly ILogger _consoleLogger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    //private readonly ICurrentUserService _currentUserService;
    private readonly IHostEnvironment _hostEnvironment;
    private bool _disposed;

    public LogService(
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        //ICurrentUserService currentUserService,
        IHostEnvironment hostEnvironment)
    {
        _httpContextAccessor = httpContextAccessor;
        //_currentUserService = currentUserService;
        _hostEnvironment = hostEnvironment;

        try
        {
            var connectionString = configuration.GetConnectionString("LoggingDb");
            var columnOptions = new ColumnOptions
            {
                AdditionalColumns = new List<SqlColumn>
                {
                    new SqlColumn("UserId", SqlDbType.Int) { AllowNull = true },
                    new SqlColumn("RequestPath", SqlDbType.NVarChar) { DataLength = 500, AllowNull = true },
                    new SqlColumn("HttpMethod", SqlDbType.NVarChar) { DataLength = 10, AllowNull = true },
                    new SqlColumn("IPAddress", SqlDbType.NVarChar) { DataLength = 50, AllowNull = true },
                    new SqlColumn("UserAgent", SqlDbType.NVarChar) { DataLength = 500, AllowNull = true },
                    new SqlColumn("Duration", SqlDbType.BigInt) { AllowNull = true },
                    new SqlColumn("Source", SqlDbType.NVarChar) { DataLength = 200, AllowNull = true },
                    new SqlColumn("MachineName", SqlDbType.NVarChar) { DataLength = 100, AllowNull = true },
                    new SqlColumn("Environment", SqlDbType.NVarChar) { DataLength = 50, AllowNull = true }
                }
            };

            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Add(StandardColumn.Properties);
            columnOptions.Properties.DataLength = -1;

            _dbLogger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", _hostEnvironment.EnvironmentName)
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    sinkOptions: new MSSqlServerSinkOptions()
                    {
                        TableName = "Logs",
                        AutoCreateSqlTable = true,
                        AutoCreateSqlDatabase = false,
                        SchemaName = "dbo"
                    },
                    columnOptions: columnOptions)
                .CreateLogger();
        }
        catch (Exception ex)
        {
            _dbLogger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            _dbLogger.Error(ex, "Failed to initialize database logger");
        }

        _consoleLogger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
    }

    public void DbLog(string message, LogLevel level = LogLevel.Information)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        try
        {
            var logEventLevel = ConvertToLogEventLevel(level);
            //using (LogContext.PushProperty("UserId", _currentUserService.UserId))
            using (LogContext.PushProperty("RequestPath", _httpContextAccessor.HttpContext?.Request.Path))
            using (LogContext.PushProperty("HttpMethod", _httpContextAccessor.HttpContext?.Request.Method))
            using (LogContext.PushProperty("IPAddress", GetIpAddress()))
            using (LogContext.PushProperty("UserAgent", _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString()))
            using (LogContext.PushProperty("Source", GetSource()))
            {
                _dbLogger.Write(logEventLevel, message);
            }
        }
        catch (Exception ex)
        {
            _consoleLogger.Error(ex, "Failed to write to database logger");
        }
    }

    public void ConsoleLog(string message, LogLevel level = LogLevel.Information)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        try
        {
            var logEventLevel = ConvertToLogEventLevel(level);
            _consoleLogger.Write(logEventLevel, message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to write console log: {ex.Message}");
        }
    }

    private static LogEventLevel ConvertToLogEventLevel(LogLevel level)
    {
        return level switch
        {
            LogLevel.Trace => LogEventLevel.Verbose,
            LogLevel.Debug => LogEventLevel.Debug,
            LogLevel.Information => LogEventLevel.Information,
            LogLevel.Warning => LogEventLevel.Warning,
            LogLevel.Error => LogEventLevel.Error,
            LogLevel.Critical => LogEventLevel.Fatal,
            LogLevel.None => LogEventLevel.Information,
            _ => LogEventLevel.Information
        };
    }

    private string? GetIpAddress()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            return null;

        var ipAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(ipAddress))
        {
            var addresses = ipAddress.Split(',');
            if (addresses.Length > 0)
                return addresses[0].Trim();
        }

        return httpContext.Connection.RemoteIpAddress?.ToString();
    }

    private string? GetSource()
    {
        var stackTrace = new System.Diagnostics.StackTrace(skipFrames: 2);
        var frame = stackTrace.GetFrame(0);
        return frame?.GetMethod()?.DeclaringType?.FullName;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        try
        {
            (_dbLogger as IDisposable)?.Dispose();
            (_consoleLogger as IDisposable)?.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error disposing loggers: {ex.Message}");
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}