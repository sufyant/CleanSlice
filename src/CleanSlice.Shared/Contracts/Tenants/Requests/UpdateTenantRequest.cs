namespace CleanSlice.Shared.Contracts.Tenants.Requests;

public sealed record UpdateTenantRequest(
    Guid TenantId,
    string Name,
    string Domain,
    string Slug,
    string ConnectionString
    );
