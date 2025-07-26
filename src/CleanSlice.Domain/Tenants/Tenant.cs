using CleanSlice.Domain.Common.Exceptions;
using CleanSlice.Domain.Tenants.Events;
using CleanSlice.Shared.Entities;

namespace CleanSlice.Domain.Tenants;

public sealed class Tenant : AuditableEntityWithSoftDelete
{
    public string Name { get; private set; } = string.Empty;
    public string Domain { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string ConnectionString { get; private set; } = string.Empty;

    private Tenant() { }

    private Tenant(Guid id, string name, string domain, string slug, string connectionString)
    {
        Id = id;
        Name = name;
        Domain = domain;
        Slug = slug;
        ConnectionString = connectionString;
    }

    public static Tenant Create(Guid id, string name, string domain, string slug, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException(nameof(name), "Name cannot be empty");

        if (string.IsNullOrWhiteSpace(domain))
            throw new ValidationException(nameof(domain), "Domain cannot be empty");

        if (string.IsNullOrWhiteSpace(slug))
            throw new ValidationException(nameof(slug), "Slug cannot be empty");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ValidationException(nameof(connectionString), "Connection string cannot be empty");

        var tenant = new Tenant(id, name, domain, slug, connectionString);

        tenant.RaiseDomainEvent(new TenantCreatedDomainEvent(id));

        return tenant;
    }

    public void Update(Guid id, string name, string domain, string slug, string connectionString)
    {
        if (IsActive)
            throw new BusinessRuleViolationException("Cannot update deleted tenant");

        if (id != Id)
            throw new ValidationException(nameof(id), "Tenant ID mismatch");

        UpdateName(name);
        UpdateDomain(domain);
        UpdateSlug(slug);
        UpdateConnectionString(connectionString);

        RaiseDomainEvent(new TenantUpdatedDomainEvent(id));
    }

    private void UpdateName(string name)
    {
        if (IsActive)
            throw new BusinessRuleViolationException("Cannot update deleted tenant");

        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException(nameof(name), "Name cannot be empty");

        Name = name;
    }

    private void UpdateDomain(string domain)
    {
        if (IsActive)
            throw new BusinessRuleViolationException("Cannot update deleted tenant");

        if (string.IsNullOrWhiteSpace(domain))
            throw new ValidationException(nameof(domain), "Domain cannot be empty");

        Domain = domain;
    }

    private void UpdateSlug(string slug)
    {
        if (IsActive)
            throw new BusinessRuleViolationException("Cannot update deleted tenant");

        if (string.IsNullOrWhiteSpace(slug))
            throw new ValidationException(nameof(slug), "Slug cannot be empty");

        Slug = slug;
    }

    private void UpdateConnectionString(string connectionString)
    {
        if (IsActive)
            throw new BusinessRuleViolationException("Cannot update deleted tenant");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ValidationException(nameof(connectionString), "Connection string cannot be empty");

        ConnectionString = connectionString;
    }
}
