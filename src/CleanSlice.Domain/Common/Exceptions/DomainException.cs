namespace CleanSlice.Domain.Common.Exceptions;

/// <summary>
/// Base exception for all domain-related errors.
/// This exception should be thrown when business rules are violated or domain invariants are broken.
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }

    protected DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
