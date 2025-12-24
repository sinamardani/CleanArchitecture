using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace Shared.Behaviors;

public class PerformanceBehavior<TRequest, TResponse>(ILogService logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Stopwatch _timer = new();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;

            logger.DbLog(
                $"Long Running Request: {requestName} ({elapsedMilliseconds} milliseconds) {@request}",
                LogLevel.Warning);
        }

        return response;
    }
}

