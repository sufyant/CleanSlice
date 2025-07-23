namespace CleanSlice.Shared.Exceptions.Infrastructure;

/// <summary>
/// Thrown when an entity is not found in the database.
/// This exception should be thrown when attempting to retrieve an entity that does not exist.
/// </summary>
public sealed class EntityNotFoundException : InfrastructureException
{
    public EntityNotFoundException(string message) : base(message)
    {
    }

    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
