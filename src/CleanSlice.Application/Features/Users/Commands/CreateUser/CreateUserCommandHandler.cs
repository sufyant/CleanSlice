using CleanSlice.Application.Abstractions.Authentication;
using CleanSlice.Application.Abstractions.Keycloak;
using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Application.Features.Users.DTOs;
using CleanSlice.Domain.Users;
using CleanSlice.Shared.Results;
using CleanSlice.Shared.Results.Errors;

namespace CleanSlice.Application.Features.Users.Commands.CreateUser;

internal sealed class CreateUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IKeycloakService keycloakService,
    IUserContext userContext
    ) : ICommandHandler<CreateUserCommand, UserDto>
{
    public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Check if user already exists
        if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            return UserErrors.EmailAlreadyExists;
        }

        // Create user in Keycloak first
        var keycloakSuccess = await keycloakService.CreateUserAsync(
            request.Email, 
            request.FirstName, 
            request.LastName, 
            request.Password, 
            cancellationToken);

        if (!keycloakSuccess)
        {
            return UserErrors.FailedToCreateInKeycloak;
        }

        // Get the Keycloak user ID
        var identityId = await keycloakService.GetUserIdByEmailAsync(request.Email, cancellationToken);
        if (string.IsNullOrEmpty(identityId))
        {
            return UserErrors.FailedToGetKeycloakUserId;
        }

        // Create user in our domain
        var user = User.Create(
            Guid.NewGuid(),
            userContext.TenantId,
            identityId,
            request.Email,
            request.FirstName,
            request.LastName);

        // Assign roles
        foreach (var roleName in request.Roles)
        {
            var role = await roleRepository.GetByNameAsync(roleName, userContext.TenantId, cancellationToken);
            if (role != null)
            {
                user.AssignRole(role);
                
                // Also assign role in Keycloak
                await keycloakService.AssignRoleToUserAsync(identityId, roleName, cancellationToken);
            }
        }

        await userRepository.AddAsync(user, cancellationToken);

        var userDto = new UserDto(
            user.Id,
            user.IdentityId,
            user.TenantId,
            user.Email,
            user.FirstName,
            user.LastName,
            user.FullName,
            user.IsActive,
            request.Roles);

        return Result.Success(userDto);
    }
}
