namespace CleanSlice.Shared.Contracts.Tenants.Responses;

public sealed record TenantResponse(
    Guid Id,
    string Name,
    string Domain,
    string Slug,
    string ConnectionString
    );
