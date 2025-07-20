using CleanSlice.Shared.Interfaces;

namespace CleanSlice.Shared.Entities;

public abstract class TenantBaseEntity : BaseEntity, IMustHaveTenant
{
    public Guid TenantId { get; set; }
}
