namespace CleanSlice.Shared.Contracts.Admin.Tenants;

public sealed record UpdateTenantResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Domain { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public DateTime LastModifiedAt { get; init; }
}
