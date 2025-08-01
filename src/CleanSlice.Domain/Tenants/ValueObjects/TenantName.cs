using CleanSlice.Domain.Common.Exceptions;

namespace CleanSlice.Domain.Tenants.ValueObjects;

public sealed class TenantName
{
    public string Value { get; }

    private TenantName(string value)
    {
        Value = value;
    }

    public static TenantName Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException(nameof(name), "Tenant name cannot be empty");

        if (name.Length > 100)
            throw new ValidationException(nameof(name), "Tenant name cannot exceed 100 characters");

        if (name.Length < 2)
            throw new ValidationException(nameof(name), "Tenant name must be at least 2 characters");

        var normalizedName = name.Trim();

        return new TenantName(normalizedName);
    }

    public override string ToString() => Value;

    public static implicit operator string(TenantName tenantName) => tenantName.Value;

    public override bool Equals(object? obj)
    {
        return obj is TenantName other && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
