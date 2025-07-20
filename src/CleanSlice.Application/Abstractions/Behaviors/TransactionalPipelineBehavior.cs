using System.Data;
using CleanSlice.Application.Abstractions.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSlice.Application.Abstractions.Behaviors;

internal sealed class TransactionalPipelineBehavior<TRequest, TResponse>(
    IUnitOfWork unitOfWork,
    ILogger<TransactionalPipelineBehavior<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Beginning transacion for {RequestName}", typeof(TRequest).Name);
        
        using IDbTransaction transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        TResponse response = await next(cancellationToken);
        
        transaction.Commit();
        
        logger.LogInformation("Transaction committed for {RequestName}", typeof(TRequest).Name);

        return response;
    }
}
