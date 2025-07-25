namespace CleanSlice.Shared.Contracts.Tenants.Requests;

public sealed record CreateTenantRequest(
    string Name,
    string Domain,
    string Slug,
    string ConnectionString 
    );
