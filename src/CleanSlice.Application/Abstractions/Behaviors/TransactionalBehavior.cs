using System.Data;
using CleanSlice.Application.Abstractions.Data;
using CleanSlice.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSlice.Application.Abstractions.Behaviors;

internal sealed class TransactionalBehavior<TRequest, TResponse>(
    IUnitOfWork unitOfWork,
    ILogger<TransactionalBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Skip transaction for non-transactional requests
        if (request is not ITransactionalCommand)
        {
            return await next(cancellationToken);
        }

        string requestName = typeof(TRequest).Name;

        logger.LogInformation("Beginning transaction for {RequestName}", requestName);

        using IDbTransaction transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            TResponse response = await next(cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            logger.LogInformation("Transaction committed for {RequestName}", requestName);

            return response;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);

            logger.LogWarning("Transaction rolled back for {RequestName}", requestName);

            throw; // Let ExceptionHandlingBehavior handle the conversion to Result
        }
    }
}
