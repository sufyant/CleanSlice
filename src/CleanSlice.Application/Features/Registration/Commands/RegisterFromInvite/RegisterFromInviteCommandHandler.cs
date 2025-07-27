using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Application.Features.Registration.DTOs;
using CleanSlice.Domain.Users;
using CleanSlice.Shared.Results;
using CleanSlice.Shared.Results.Errors;

namespace CleanSlice.Application.Features.Registration.Commands.RegisterFromInvite;

internal sealed class RegisterFromInviteCommandHandler(
    IInvitationRepository invitationRepository,
    IUserRepository userRepository,
    IRoleRepository roleRepository
    ) : ICommandHandler<RegisterFromInviteCommand, RegistrationDto>
{
    public async Task<Result<RegistrationDto>> Handle(RegisterFromInviteCommand request, CancellationToken cancellationToken)
    {
        // Validate invitation token
        var invitation = await invitationRepository.GetByTokenAsync(request.Token, cancellationToken);
        
        if (invitation == null)
        {
            return InvitationErrors.NotFound;
        }

        if (invitation.IsExpired)
        {
            return InvitationErrors.Expired;
        }

        if (invitation.IsUsed)
        {
            return InvitationErrors.AlreadyUsed;
        }

        // Verify email matches invitation
        if (!string.Equals(invitation.Email.Value, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            return InvitationErrors.EmailMismatch;
        }

        // Check if user already exists
        if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            return UserErrors.AlreadyExists;
        }

        // TODO: Create user in Azure Entra ID External ID
        // This would typically involve calling Microsoft Graph API
        // For now, we'll use a placeholder identity ID
        var azureIdentityId = Guid.NewGuid().ToString(); // This should be the actual Azure user ID

        // Create user in local database
        var user = User.Create(
            Guid.NewGuid(),
            invitation.TenantId,
            azureIdentityId,
            request.Email,
            request.FirstName,
            request.LastName);

        // Assign role from invitation
        var role = await roleRepository.GetByIdAsync(invitation.RoleId, cancellationToken);
        if (role != null)
        {
            user.AssignRole(role);
        }

        await userRepository.AddAsync(user, cancellationToken);

        // Mark invitation as used
        invitation.MarkAsUsed(user.Id);

        var response = new RegistrationDto(
            user.Id,
            user.IdentityId,
            user.TenantId,
            user.Email.Value,
            user.FirstName,
            user.LastName,
            user.FullName,
            role?.Name.Value ?? "Unknown"
        );

        return Result.Success(response);
    }
}
