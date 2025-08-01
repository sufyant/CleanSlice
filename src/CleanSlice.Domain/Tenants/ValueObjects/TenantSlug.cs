using System.Text.RegularExpressions;
using CleanSlice.Domain.Common.Exceptions;

namespace CleanSlice.Domain.Tenants.ValueObjects;

public sealed class TenantSlug
{
    private static readonly Regex SlugRegex = new(
        @"^[a-z0-9]+(?:-[a-z0-9]+)*$",
        RegexOptions.Compiled);

    public string Value { get; }

    private TenantSlug(string value)
    {
        Value = value;
    }

    public static TenantSlug Create(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            throw new ValidationException(nameof(slug), "Slug cannot be empty");

        var normalizedSlug = slug.Trim().ToLowerInvariant();

        if (normalizedSlug.Length > 50)
            throw new ValidationException(nameof(slug), "Slug cannot exceed 50 characters");

        if (normalizedSlug.Length < 2)
            throw new ValidationException(nameof(slug), "Slug must be at least 2 characters");

        if (!SlugRegex.IsMatch(normalizedSlug))
            throw new ValidationException(nameof(slug), "Slug can only contain lowercase letters, numbers, and hyphens");

        // Reserved slugs
        var reservedSlugs = new[] { "api", "admin", "www", "app", "mail", "ftp", "localhost", "test" };
        if (reservedSlugs.Contains(normalizedSlug))
            throw new ValidationException(nameof(slug), "This slug is reserved and cannot be used");

        return new TenantSlug(normalizedSlug);
    }

    public override string ToString() => Value;

    public static implicit operator string(TenantSlug tenantSlug) => tenantSlug.Value;

    public override bool Equals(object? obj)
    {
        return obj is TenantSlug other && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(TenantSlug? left, TenantSlug? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TenantSlug? left, TenantSlug? right)
    {
        return !Equals(left, right);
    }
}
