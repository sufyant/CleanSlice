using CleanSlice.Shared.Results;
using MediatR;

namespace CleanSlice.Application.Abstractions.Messaging;

public interface ITransactionalCommand : ICommand;

public interface ITransactionalCommand<TResponse> : IRequest<Result<TResponse>>, ITransactionalCommand;
