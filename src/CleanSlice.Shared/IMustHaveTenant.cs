namespace CleanSlice.Shared;

public interface IMustHaveTenant
{
    public Guid TenantId { get; set; }
}
