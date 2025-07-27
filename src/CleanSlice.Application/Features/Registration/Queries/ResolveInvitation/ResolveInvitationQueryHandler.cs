using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Application.Features.Registration.DTOs;
using CleanSlice.Shared.Results;
using CleanSlice.Shared.Results.Errors;
using Microsoft.Extensions.Logging;

namespace CleanSlice.Application.Features.Registration.Queries.ResolveInvitation;

internal sealed class ResolveInvitationQueryHandler(
    IInvitationRepository invitationRepository,
    IRoleRepository roleRepository,
    ILogger<ResolveInvitationQueryHandler> logger) : IQueryHandler<ResolveInvitationQuery, InvitationDetailsDto>
{
    public async Task<Result<InvitationDetailsDto>> Handle(ResolveInvitationQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            return InvitationErrors.InvalidToken;
        }
        
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

        // Get role information
        var role = await roleRepository.GetByIdAsync(invitation.RoleId, cancellationToken);
        
        var response = new InvitationDetailsDto(
            invitation.Id,
            invitation.Email.Value,
            invitation.TenantId,
            role?.Name.Value ?? "Unknown",
            invitation.InvitedBy,
            invitation.CreatedAt,
            invitation.ExpiresAt
        );

        return Result.Success(response);
    }
}
