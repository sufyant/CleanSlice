using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Features.Roles.DTOs;

namespace CleanSlice.Application.Features.Roles.Commands.CreateRole;

public sealed record CreateRoleCommand(
    string Name,
    string Description,
    IEnumerable<string> Permissions
    ) : ICommand<RoleDto>;
