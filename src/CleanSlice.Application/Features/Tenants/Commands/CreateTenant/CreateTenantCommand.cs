using CleanSlice.Application.Abstractions.Messaging;

namespace CleanSlice.Application.Features.Tenants.Commands.CreateTenant;

public sealed record CreateTenantCommand(
    string Name,
    string Domain,
    string Slug,
    string ConnectionString 
    ) : ICommand<Guid>;
