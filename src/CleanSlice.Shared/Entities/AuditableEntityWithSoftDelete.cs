using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Shared.Entities;

public abstract class AuditableEntityWithSoftDelete : BaseEntity, IAuditableEntity, ISoftDelete
{
    public Guid CreatedBy { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; }
    public Guid? LastModifiedBy { get; protected set; }
    public DateTimeOffset? LastModifiedAt { get; protected set; }
    public DateTimeOffset? DeletedAt { get; protected set; }
    public Guid? DeletedBy { get; protected set; }
    protected bool IsActive => !DeletedAt.HasValue;

    public void MarkAsDeleted(Guid deletedBy)
    {
        if (IsActive) return;
        DeletedAt = DateTimeOffset.UtcNow;
        DeletedBy = deletedBy;
    }

    public void Restore()
    {
        if (!IsActive) return;
        DeletedAt = null;
        DeletedBy = null;
    }
}
