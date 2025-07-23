namespace CleanSlice.Domain.Common.Exceptions;

/// <summary>
/// Thrown when domain validation fails.
/// This exception should be thrown when domain entity validation rules are not satisfied.
/// </summary>
public sealed class ValidationException : DomainException
{
    public ValidationException(string message) : base(message)
    {
    }
    
    public ValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
