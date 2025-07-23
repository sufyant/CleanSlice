namespace CleanSlice.Domain.Common.Exceptions;

/// <summary>
/// Thrown when a business rule is violated in the domain.
/// This exception should be thrown when domain invariants or business constraints are not satisfied.
/// </summary>
public sealed class BusinessRuleViolationException : DomainException
{
    public BusinessRuleViolationException(string message) : base(message)
    {
    }

    public BusinessRuleViolationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
