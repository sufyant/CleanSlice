namespace CleanSlice.Shared.Exceptions.Infrastructure;

/// <summary>
/// Thrown when a database concurrency conflict occurs.
/// This exception should be thrown when optimistic concurrency control fails.
/// </summary>
public sealed class ConcurrencyException : InfrastructureException
{
    public ConcurrencyException(string message) : base(message)
    {
    }
    
    public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
