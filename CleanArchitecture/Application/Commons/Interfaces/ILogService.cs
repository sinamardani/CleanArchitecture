using Microsoft.Extensions.Logging;

namespace Application.Commons.Interfaces;

public interface ILogService
{
    void DbLog(string message, LogLevel level = LogLevel.Information);
    void ConsoleLog(string message, LogLevel level = LogLevel.Information);
}