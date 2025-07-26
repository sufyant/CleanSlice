using CleanSlice.Domain.Common.Exceptions;

namespace CleanSlice.Domain.Users.ValueObjects;

public sealed class RoleName
{
    public string Value { get; }

    private RoleName(string value)
    {
        Value = value;
    }

    public static RoleName Create(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            throw new ValidationException(nameof(roleName), "Role name cannot be empty");

        if (roleName.Length > 100)
            throw new ValidationException(nameof(roleName), "Role name cannot exceed 100 characters");

        var normalizedRoleName = roleName.Trim().ToUpperInvariant();

        return new RoleName(normalizedRoleName);
    }

    public override string ToString() => Value;

    public static implicit operator string(RoleName roleName) => roleName.Value;

    public override bool Equals(object? obj)
    {
        return obj is RoleName other && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
