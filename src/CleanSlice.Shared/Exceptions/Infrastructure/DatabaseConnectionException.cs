namespace CleanSlice.Shared.Exceptions.Infrastructure;

/// <summary>
/// Thrown when a database connection fails.
/// This exception should be thrown when unable to establish or maintain a connection to the database.
/// </summary>
public sealed class DatabaseConnectionException : InfrastructureException
{
    public DatabaseConnectionException(string message) : base(message)
    {
    }
    
    public DatabaseConnectionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
