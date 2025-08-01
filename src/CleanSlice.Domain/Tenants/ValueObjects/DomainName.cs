using System.Text.RegularExpressions;
using CleanSlice.Domain.Common.Exceptions;

namespace CleanSlice.Domain.Tenants.ValueObjects;

public sealed class DomainName
{
    private static readonly Regex DomainRegex = new(
        @"^[a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?(\.[a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?)*$",
        RegexOptions.Compiled);

    public string Value { get; }

    private DomainName(string value)
    {
        Value = value;
    }

    public static DomainName Create(string domain)
    {
        if (string.IsNullOrWhiteSpace(domain))
            throw new ValidationException(nameof(domain), "Domain cannot be empty");

        var normalizedDomain = domain.Trim().ToLowerInvariant();

        if (normalizedDomain.Length > 253)
            throw new ValidationException(nameof(domain), "Domain cannot exceed 253 characters");

        if (!DomainRegex.IsMatch(normalizedDomain))
            throw new ValidationException(nameof(domain), "Invalid domain format");

        return new DomainName(normalizedDomain);
    }

    public override string ToString() => Value;

    public static implicit operator string(DomainName domainName) => domainName.Value;

    public override bool Equals(object? obj)
    {
        return obj is DomainName other && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(DomainName? left, DomainName? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(DomainName? left, DomainName? right)
    {
        return !Equals(left, right);
    }
}
