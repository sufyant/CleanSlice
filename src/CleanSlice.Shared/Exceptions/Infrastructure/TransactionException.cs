namespace CleanSlice.Shared.Exceptions.Infrastructure;

/// <summary>
/// Thrown when a database transaction fails.
/// This exception should be thrown when a transaction cannot be committed or rolled back.
/// </summary>
public sealed class TransactionException : InfrastructureException
{
    public TransactionException(string message) : base(message)
    {
    }

    public TransactionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
