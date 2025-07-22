namespace CleanSlice.Shared.Interfaces;

public interface IAuditableEntity
{
    public Guid CreatedBy { get; }
    public DateTimeOffset CreatedAt { get; }
    public Guid? LastModifiedBy { get; }
    public DateTimeOffset? LastModifiedAt { get; }
}
