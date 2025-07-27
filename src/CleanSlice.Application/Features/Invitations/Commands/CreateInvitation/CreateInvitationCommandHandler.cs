using CleanSlice.Application.Abstractions.Authentication;
using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Application.Features.Invitations.DTOs;
using CleanSlice.Domain.Users;
using CleanSlice.Shared.Results;
using CleanSlice.Shared.Results.Errors;

namespace CleanSlice.Application.Features.Invitations.Commands.CreateInvitation;

internal sealed class CreateInvitationCommandHandler(
    IInvitationRepository invitationRepository,
    IRoleRepository roleRepository,
    IUserRepository userRepository,
    IUserContext userContext
    ) : ICommandHandler<CreateInvitationCommand, InvitationDto>
{
    public async Task<Result<InvitationDto>> Handle(CreateInvitationCommand request, CancellationToken cancellationToken)
    {
        // Check if user already exists
        if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            return UserErrors.AlreadyExists;
        }

        // Check if there's already a pending invitation for this email
        var existingInvitation = await invitationRepository.GetPendingByEmailAsync(request.Email, userContext.TenantId, cancellationToken);
        if (existingInvitation != null)
        {
            return InvitationErrors.AlreadyExists;
        }

        // Validate role exists
        var role = await roleRepository.GetByNameAsync(request.RoleName, userContext.TenantId, cancellationToken);
        if (role == null)
        {
            return RoleErrors.NotFound;
        }

        // Generate invitation token
        var token = Guid.NewGuid().ToString("N"); // 32 character token without hyphens
        
        // Create invitation
        var invitation = Invitation.Create(
            Guid.NewGuid(),
            userContext.TenantId,
            request.Email,
            role.Id,
            userContext.UserId,
            token,
            DateTime.UtcNow.AddDays(request.ExpirationDays ?? 7) // Default 7 days
        );

        await invitationRepository.AddAsync(invitation, cancellationToken);

        var response = new InvitationDto(
            invitation.Id,
            invitation.Email.Value,
            invitation.TenantId,
            role.Name.Value,
            invitation.Token,
            invitation.InvitedBy,
            invitation.CreatedAt,
            invitation.ExpiresAt,
            invitation.IsUsed,
            invitation.UsedAt,
            invitation.UsedBy
        );

        return Result.Success(response);
    }
}
