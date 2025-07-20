using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Shared.Entities;

public abstract class AuditableEntity : BaseEntity, IAuditableEntity
{
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
}
