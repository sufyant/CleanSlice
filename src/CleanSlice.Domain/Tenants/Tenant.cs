using CleanSlice.Domain.Common.Exceptions;
using CleanSlice.Domain.Tenants.Events;
using CleanSlice.Domain.Tenants.ValueObjects;
using CleanSlice.Shared.Entities;

namespace CleanSlice.Domain.Tenants;

public sealed class Tenant : AuditableEntityWithSoftDelete
{
    public TenantName Name { get; private set; } = null!;
    public DomainName Domain { get; private set; } = null!;
    public TenantSlug Slug { get; private set; } = null!;
    public string ConnectionString { get; private set; } = string.Empty;

    private Tenant() { }

    private Tenant(Guid id, TenantName name, DomainName domain, TenantSlug slug, string connectionString)
    {
        Id = id;
        Name = name;
        Domain = domain;
        Slug = slug;
        ConnectionString = connectionString;
    }

    public static Tenant Create(Guid id, string name, string domain, string slug, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ValidationException(nameof(connectionString), "Connection string cannot be empty");

        var tenantName = TenantName.Create(name);
        var domainName = DomainName.Create(domain);
        var tenantSlug = TenantSlug.Create(slug);

        var tenant = new Tenant(id, tenantName, domainName, tenantSlug, connectionString);

        tenant.RaiseDomainEvent(new TenantCreatedDomainEvent(id));

        return tenant;
    }

    public void Update(string name, string domain, string slug, string connectionString)
    {
        if (!IsActive)
            throw new DomainInvariantViolationException("Cannot update deleted tenant");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ValidationException(nameof(connectionString), "Connection string cannot be empty");

        Name = TenantName.Create(name);
        Domain = DomainName.Create(domain);
        Slug = TenantSlug.Create(slug);
        ConnectionString = connectionString;

        RaiseDomainEvent(new TenantUpdatedDomainEvent(Id));
    }
}
