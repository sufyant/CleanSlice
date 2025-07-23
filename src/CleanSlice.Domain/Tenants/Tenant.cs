using CleanSlice.Domain.Common.Exceptions;
using CleanSlice.Domain.Tenants.Events;
using CleanSlice.Shared.Entities;

namespace CleanSlice.Domain.Tenants;

public sealed class Tenant : AuditableEntityWithSoftDelete
{
    public string Name { get; private set; } = string.Empty;
    public string ConnectionString { get; private set; } = string.Empty;

    private Tenant() { }

    private Tenant(Guid id, string name, string connectionString)
    {
        Id = id;
        Name = name;
        ConnectionString = connectionString;
    }

    public static Tenant Create(Guid id, string name, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException(nameof(name), "Name cannot be empty");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ValidationException(nameof(connectionString), "Connection string cannot be empty");

        var tenant = new Tenant(id, name, connectionString);

        tenant.RaiseDomainEvent(new TenantCreatedDomainEvent(id, name));

        return tenant;
    }

    public void UpdateName(string name)
    {
        if (IsDeleted)
            throw new BusinessRuleViolationException("Cannot update deleted tenant");

        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException(nameof(name), "Name cannot be empty");

        Name = name;
    }

    public void UpdateConnectionString(string connectionString)
    {
        if (IsDeleted)
            throw new BusinessRuleViolationException("Cannot update deleted tenant");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ValidationException(nameof(connectionString), "Connection string cannot be empty");

        ConnectionString = connectionString;
    }
}
