namespace CleanSlice.Domain.Common.Exceptions;

/// <summary>
/// Thrown when a domain invariant is violated.
/// This exception should be thrown when the internal state of a domain entity becomes inconsistent.
/// </summary>
public sealed class DomainInvariantViolationException : DomainException
{
    public DomainInvariantViolationException(string message) : base(message)
    {
    }

    public DomainInvariantViolationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
