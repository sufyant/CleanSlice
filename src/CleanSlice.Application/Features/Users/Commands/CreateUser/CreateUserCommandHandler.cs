using CleanSlice.Application.Abstractions.Authentication;

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

        // Note: Azure Entra ID user creation will be handled via invitation flow
        // For now, we'll use a placeholder identity ID that will be updated during sync
        var identityId = Guid.NewGuid().ToString(); // Temporary ID, will be replaced with Azure ID

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
                // Note: Azure Entra ID role assignment will be handled separately
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
