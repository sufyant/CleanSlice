using CleanSlice.Shared;
using MediatR;

namespace CleanSlice.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
    
}
