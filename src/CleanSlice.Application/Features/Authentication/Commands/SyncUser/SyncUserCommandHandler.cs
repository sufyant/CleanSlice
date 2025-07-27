using CleanSlice.Application.Abstractions.Authentication;
using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Application.Features.Authentication.DTOs;
using CleanSlice.Domain.Users;
using CleanSlice.Shared.Results;

namespace CleanSlice.Application.Features.Authentication.Commands.SyncUser;

internal sealed class SyncUserCommandHandler(
    IUserRepository userRepository,
    IUserContext userContext
    ) : ICommandHandler<SyncUserCommand, UserSyncDto>
{
    public async Task<Result<UserSyncDto>> Handle(SyncUserCommand request, CancellationToken cancellationToken)
    {
        var identityId = userContext.IdentityId;
        var email = userContext.Email;
        var name = userContext.Name;
        
        // Parse first and last name from full name
        var nameParts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var firstName = nameParts.FirstOrDefault() ?? "";
        var lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";

        // Check if user already exists in local database
        var existingUser = await userRepository.GetByIdentityIdAsync(identityId, cancellationToken);
        
        if (existingUser != null)
        {
            // User exists, update if needed
            if (existingUser.Email.Value != email || 
                existingUser.FirstName != firstName || 
                existingUser.LastName != lastName)
            {
                existingUser.Update(email, firstName, lastName);
            }

            return Result.Success(new UserSyncDto(
                existingUser.Id,
                existingUser.IdentityId,
                existingUser.TenantId,
                existingUser.Email.Value,
                existingUser.FirstName,
                existingUser.LastName,
                existingUser.FullName,
                existingUser.IsActive,
                false // not newly created
            ));
        }

        // User doesn't exist, create new one
        var tenantId = userContext.TenantId;
        
        var newUser = User.Create(
            Guid.NewGuid(),
            tenantId,
            identityId,
            email,
            firstName,
            lastName);

        await userRepository.AddAsync(newUser, cancellationToken);

        return Result.Success(new UserSyncDto(
            newUser.Id,
            newUser.IdentityId,
            newUser.TenantId,
            newUser.Email.Value,
            newUser.FirstName,
            newUser.LastName,
            newUser.FullName,
            newUser.IsActive,
            true // newly created
        ));
        
    }
}
