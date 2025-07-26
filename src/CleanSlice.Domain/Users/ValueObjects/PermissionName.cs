using CleanSlice.Domain.Common.Exceptions;

namespace CleanSlice.Domain.Users.ValueObjects;

public sealed class PermissionName
{
    public string Value { get; }

    private PermissionName(string value)
    {
        Value = value;
    }

    public static PermissionName Create(string permissionName)
    {
        if (string.IsNullOrWhiteSpace(permissionName))
            throw new ValidationException(nameof(permissionName), "Permission name cannot be empty");

        if (permissionName.Length > 100)
            throw new ValidationException(nameof(permissionName), "Permission name cannot exceed 100 characters");

        var normalizedPermissionName = permissionName.Trim().ToUpperInvariant();

        return new PermissionName(normalizedPermissionName);
    }

    public override string ToString() => Value;

    public static implicit operator string(PermissionName permissionName) => permissionName.Value;

    public override bool Equals(object? obj)
    {
        return obj is PermissionName other && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
