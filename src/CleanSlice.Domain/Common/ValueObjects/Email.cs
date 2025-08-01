using System.Text.RegularExpressions;
using CleanSlice.Domain.Common.Exceptions;

namespace CleanSlice.Domain.Common.ValueObjects;

public sealed class Email
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled);

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ValidationException(nameof(email), "Email cannot be null or empty");

        var normalizedEmail = email.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(normalizedEmail))
            throw new ValidationException(nameof(email), "Invalid email format");

        return new Email(normalizedEmail);
    }

    public string GetDomain() => Value.Split('@')[1];

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;

    public override bool Equals(object? obj)
    {
        return obj is Email other && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
