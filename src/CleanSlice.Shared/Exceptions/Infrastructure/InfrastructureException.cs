namespace CleanSlice.Shared.Exceptions.Infrastructure;

/// <summary>
/// Base exception for all infrastructure-related errors.
/// This exception should be thrown when infrastructure operations fail (database, caching, external services, etc.).
/// </summary>
public abstract class InfrastructureException : Exception
{
    protected InfrastructureException(string message) : base(message)
    {
    }

    protected InfrastructureException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
