using CleanSlice.Domain.Tenants.Events;
using CleanSlice.Shared.Entities;

namespace CleanSlice.Domain.Tenants;

public sealed class Tenant : AuditableEntityWithSoftDelete
{
    public string Name { get; private set; } = string.Empty;
    public string ConnectionString { get; private set; } = string.Empty;

    private Tenant() { }

    private Tenant(Guid id, string name, string connectionString, Guid createdBy, DateTimeOffset createdAt)
    {
        Id = id;
        Name = name;
        ConnectionString = connectionString;
        CreatedBy = createdBy;
        CreatedAt = createdAt;
    }

    public static Tenant Create(string name, string connectionString, Guid createdBy)
    {
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant(tenantId, name, connectionString, createdBy, DateTimeOffset.UtcNow);

        tenant.RaiseDomainEvent(new TenantCreatedDomainEvent(tenantId, name));

        return tenant;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
    }

    public void UpdateConnectionString(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string cannot be empty", nameof(connectionString));

        ConnectionString = connectionString;
    }
}
