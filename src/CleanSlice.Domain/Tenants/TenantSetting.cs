using CleanSlice.Domain.Common.Exceptions;
using CleanSlice.Shared.Entities;

namespace CleanSlice.Domain.Tenants;

public sealed class TenantSetting : AuditableTenantEntityWithSoftDelete
{
    public string Key { get; private set; } = string.Empty;
    public string Value { get; private set; } = string.Empty;

    private TenantSetting() { }

    private TenantSetting(Guid id, Guid tenantId, string key, string value)
    {
        Id = id;
        TenantId = tenantId;
        Key = key;
        Value = value;
    }

    public static TenantSetting Create(Guid id, Guid tenantId, string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new DomainException("Key cannot be empty");

        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Value cannot be empty");

        return new TenantSetting(id, tenantId, key, value);
    }

    public void UpdateValue(string value)
    {
        if (IsDeleted)
            throw new DomainException("Cannot update deleted tenant setting");

        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Value cannot be empty");

        Value = value;
    }
}
