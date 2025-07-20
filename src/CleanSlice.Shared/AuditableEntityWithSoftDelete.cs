namespace CleanSlice.Shared;

public abstract class AuditableEntityWithSoftDelete : BaseEntity, IAuditableEntity, ISoftDelete
{
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
