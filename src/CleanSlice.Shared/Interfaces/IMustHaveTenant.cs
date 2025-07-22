namespace CleanSlice.Shared.Interfaces;

public interface IMustHaveTenant
{
    public Guid TenantId { get; }
}
