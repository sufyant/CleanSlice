using CleanSlice.Domain.Common.Exceptions;
using CleanSlice.Shared.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;
using CleanSlice.Shared.Exceptions.Infrastructure;

namespace CleanSlice.Application.Abstractions.Behaviors;

internal sealed class ExceptionHandlingBehavior<TRequest, TResponse>(
    ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Exception occurred for {RequestName}", typeof(TRequest).Name);
            return HandleException(exception);
        }
    }

    private TResponse HandleException(Exception exception)
    {
        switch (exception)
        {
            // Domain Exceptions
            case DomainException domainException:
                return CreateFailureResult(Error.Problem("Domain.General", domainException.Message));

            // Infrastructure Exceptions
            case InfrastructureException infrastructureException:
                return CreateFailureResult(Error.Problem("Infrastructure.General", infrastructureException.Message));

            // System Exceptions
            case ArgumentNullException nullException:
                return CreateFailureResult(Error.Problem("System.NullReference", nullException.Message));

            case ArgumentException argumentException:
                return CreateFailureResult(Error.Problem("System.InvalidInput", argumentException.Message));

            case InvalidOperationException invalidOpException:
                return CreateFailureResult(Error.Problem("System.InvalidOperation", invalidOpException.Message));

            case TimeoutException timeoutException:
                return CreateFailureResult(Error.Problem("System.Timeout", timeoutException.Message));

            case HttpRequestException httpException:
                return CreateFailureResult(Error.Problem("External.ServiceUnavailable", httpException.Message));
        }

        // Unknown exceptions → Re-throw for global handler
        throw exception;
    }

    private TResponse CreateFailureResult(Error error)
    {
        // Check if TResponse is Result or Result<T>
        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(error);
        }

        if (typeof(TResponse).IsGenericType &&
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            Type resultType = typeof(TResponse).GetGenericArguments()[0];

            MethodInfo? failureMethod = typeof(Result<>)
                .MakeGenericType(resultType)
                .GetMethod(nameof(Result<object>.Failure));

            if (failureMethod is not null)
            {
                return (TResponse)failureMethod.Invoke(null, [error])!;
            }
        }

        // If TResponse is not Result type, re-throw
        throw new InvalidOperationException($"Cannot create failure result for type {typeof(TResponse).Name}");
    }
}
