namespace CleanSlice.Shared;

public abstract class TenantBaseEntity : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }
}
