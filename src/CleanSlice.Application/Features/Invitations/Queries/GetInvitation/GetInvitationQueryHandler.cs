using CleanSlice.Application.Abstractions.Authentication;
using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Application.Features.Invitations.DTOs;
using CleanSlice.Shared.Results;
using CleanSlice.Shared.Results.Errors;

namespace CleanSlice.Application.Features.Invitations.Queries.GetInvitation;

internal sealed class GetInvitationQueryHandler(
    IInvitationRepository invitationRepository,
    IRoleRepository roleRepository,
    IUserContext userContext
    ) : IQueryHandler<GetInvitationQuery, InvitationDto>
{
    public async Task<Result<InvitationDto>> Handle(GetInvitationQuery request, CancellationToken cancellationToken)
    {
        var invitation = await invitationRepository.GetByTokenAsync(request.Token, cancellationToken);
        
        if (invitation == null || invitation.TenantId != userContext.TenantId)
        {
            return InvitationErrors.NotFound;
        }

        var role = await roleRepository.GetByIdAsync(invitation.RoleId, cancellationToken);

        var response = new InvitationDto(
            invitation.Id,
            invitation.Email.Value,
            invitation.TenantId,
            role?.Name.Value ?? "Unknown",
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
