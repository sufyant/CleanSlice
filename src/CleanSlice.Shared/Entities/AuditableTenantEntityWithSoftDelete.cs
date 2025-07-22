using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Shared.Entities;

public class AuditableTenantEntityWithSoftDelete : TenantBaseEntity, IAuditableEntity, ISoftDelete
{
    public Guid CreatedBy { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; }
    public Guid? LastModifiedBy { get; protected set; }
    public DateTimeOffset? LastModifiedAt { get; protected set; }
    public DateTimeOffset? DeletedAt { get; protected set; }
    public Guid? DeletedBy { get; protected set; }
    protected bool IsDeleted => DeletedAt.HasValue;

    public void MarkAsDeleted(Guid deletedBy)
    {
        if (IsDeleted) return;
        DeletedAt = DateTimeOffset.UtcNow;
        DeletedBy = deletedBy;
    }

    public void Restore()
    {
        if (!IsDeleted) return;
        DeletedAt = null;
        DeletedBy = null;
    }
}
