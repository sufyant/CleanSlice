namespace CleanSlice.Shared.Exceptions.Infrastructure;

/// <summary>
/// Thrown when a cache operation fails.
/// This exception should be thrown when cache operations (get, set, delete) fail.
/// </summary>
public sealed class CacheException : InfrastructureException
{
    public CacheException(string message) : base(message)
    {
    }
    
    public CacheException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
