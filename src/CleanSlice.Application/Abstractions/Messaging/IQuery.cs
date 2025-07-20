using CleanSlice.Shared.Results;
using MediatR;

namespace CleanSlice.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
    
}
