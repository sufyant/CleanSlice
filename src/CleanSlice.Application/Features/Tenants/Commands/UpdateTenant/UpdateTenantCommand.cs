using CleanSlice.Application.Abstractions.Messaging;

namespace CleanSlice.Application.Features.Tenants.Commands.UpdateTenant;

public sealed record UpdateTenantCommand(
    Guid TenantId,
    string Name,
    string Domain,
    string Slug,
    string ConnectionString 
) : ICommand;
