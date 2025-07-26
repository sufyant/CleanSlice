using CleanSlice.Application.Abstractions.Authentication;
using CleanSlice.Application.Abstractions.Keycloak;
using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Application.Features.Roles.DTOs;
using CleanSlice.Domain.Users;
using CleanSlice.Shared.Results;
using CleanSlice.Shared.Results.Errors;

namespace CleanSlice.Application.Features.Roles.Commands.CreateRole;

internal sealed class CreateRoleCommandHandler(
    IRoleRepository roleRepository,
    IPermissionRepository permissionRepository,
    IKeycloakService keycloakService,
    IUserContext userContext
    ) : ICommandHandler<CreateRoleCommand, RoleDto>
{
    public async Task<Result<RoleDto>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        // Check if role already exists
        if (await roleRepository.ExistsByNameAsync(request.Name, userContext.TenantId, cancellationToken))
        {
            return RoleErrors.NameAlreadyExists;
        }

        // Create role in Keycloak first
        var keycloakSuccess = await keycloakService.CreateRoleAsync(request.Name, request.Description, cancellationToken);
        if (!keycloakSuccess)
        {
            return RoleErrors.FailedToCreateInKeycloak;
        }

        // Create role in our domain
        var role = Role.Create(
            Guid.NewGuid(),
            userContext.TenantId,
            request.Name,
            request.Description);

        // Assign permissions
        foreach (var permissionName in request.Permissions)
        {
            var permission = await permissionRepository.GetByNameAsync(permissionName, cancellationToken);
            if (permission != null)
            {
                role.AssignPermission(permission);
            }
        }

        await roleRepository.AddAsync(role, cancellationToken);

        var roleDto = new RoleDto(
            role.Id,
            role.TenantId,
            role.Name,
            role.Description,
            role.IsSystemRole,
            role.RolePermissions.Select(rp => rp.Permission.Name.Value)
            );

        return Result.Success(roleDto);
    }
}
