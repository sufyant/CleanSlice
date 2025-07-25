namespace CleanSlice.Application.Features.Tenants.DTOs;

public sealed record TenantDto(
    Guid Id,
    string Name,
    string Domain,
    string Slug,
    string ConnectionString
    );
