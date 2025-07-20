using CleanSlice.Shared;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Diagnostics;

namespace CleanSlice.Application.Abstractions.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        var timer = Stopwatch.StartNew();

        logger.LogInformation("Processing request {RequestName}", requestName);

        TResponse result = await next(cancellationToken);
        timer.Stop();

        if (result.IsSuccess)
        {
            logger.LogInformation("Completed request {RequestName} in {ElapsedMilliseconds}ms", requestName, timer.ElapsedMilliseconds);
        }
        else
        {
            using (LogContext.PushProperty("Error", result.Error, true))
            {
                logger.LogError("Completed request {RequestName} with error in {ElapsedMilliseconds}ms", requestName, timer.ElapsedMilliseconds);
            }
        }

        return result;
    }
}
