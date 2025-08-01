using CleanSlice.Domain.Common.Enums;
using CleanSlice.Domain.Common.Exceptions;

namespace CleanSlice.Domain.Common.ValueObjects;

public sealed class ExternalIdentityId
{
    public string Value { get; }
    public LoginProvider Provider { get; }

    private ExternalIdentityId(string value, LoginProvider provider)
    {
        Value = value;
        Provider = provider;
    }

    public static ExternalIdentityId Create(string value, LoginProvider provider)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException(nameof(value), "External identity ID cannot be empty");

        return new ExternalIdentityId(value.Trim(), provider);
    }

    public static ExternalIdentityId CreateForLocal(string userId)
    {
        return Create(userId, LoginProvider.Local);
    }

    public static ExternalIdentityId CreateForGoogle(string googleUserId)
    {
        return Create(googleUserId, LoginProvider.Google);
    }

    public static ExternalIdentityId CreateForMicrosoft(string microsoftUserId)
    {
        return Create(microsoftUserId, LoginProvider.Microsoft);
    }

    public bool IsLocal => Provider == LoginProvider.Local;
    public bool IsGoogle => Provider == LoginProvider.Google;
    public bool IsMicrosoft => Provider == LoginProvider.Microsoft;

    public override string ToString() => $"{Provider}:{Value}";

    public static implicit operator string(ExternalIdentityId externalId) => externalId.Value;

    public override bool Equals(object? obj)
    {
        return obj is ExternalIdentityId other &&
               Value == other.Value &&
               Provider == other.Provider;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Provider);
    }

    public static bool operator ==(ExternalIdentityId? left, ExternalIdentityId? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ExternalIdentityId? left, ExternalIdentityId? right)
    {
        return !Equals(left, right);
    }
}
