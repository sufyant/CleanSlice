namespace CleanSlice.Shared.Exceptions.Infrastructure;

/// <summary>
/// AuthenticationException is thrown when an authentication error occurs.
/// This exception should be thrown when authentication fails (e.g., invalid credentials, expired token, etc.).
/// </summary>
public sealed class AuthenticationException : InfrastructureException
{
    public AuthenticationException(string message) : base(message)
    {
    }

    public AuthenticationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
