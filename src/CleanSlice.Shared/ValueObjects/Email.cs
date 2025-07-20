using System.Text.RegularExpressions;

namespace CleanSlice.Shared.ValueObjects;

public sealed class Email : ValueObject
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
            throw new ArgumentException("Email cannot be null or empty", nameof(email));

        var normalizedEmail = email.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(normalizedEmail))
            throw new ArgumentException("Invalid email format", nameof(email));

        return new Email(normalizedEmail);
    }

    public string GetDomain() => Value.Split('@')[1];

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}
