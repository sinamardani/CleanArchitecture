using Microsoft.Extensions.Logging;

namespace Shared.Interfaces;

public interface ILogService
{
    void DbLog(string message, LogLevel level = LogLevel.Information);
    void ConsoleLog(string message, LogLevel level = LogLevel.Information);
}

