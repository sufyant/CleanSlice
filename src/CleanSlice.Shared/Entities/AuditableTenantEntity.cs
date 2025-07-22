using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Shared.Entities;

public class AuditableTenantEntity : TenantBaseEntity, IAuditableEntity
{
    public Guid CreatedBy { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; }
    public Guid? LastModifiedBy { get; protected set; }
    public DateTimeOffset? LastModifiedAt { get; protected set; }
}
