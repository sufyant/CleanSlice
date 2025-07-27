using CleanSlice.Application.Abstractions.Authentication;
using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Application.Features.Invitations.DTOs;
using CleanSlice.Shared.Results;

namespace CleanSlice.Application.Features.Invitations.Queries.GetInvitations;

internal sealed class GetInvitationsQueryHandler(
    IInvitationRepository invitationRepository,
    IRoleRepository roleRepository,
    IUserContext userContext
    ) : IQueryHandler<GetInvitationsQuery, PagedResult<InvitationDto>>
{
    public async Task<Result<PagedResult<InvitationDto>>> Handle(GetInvitationsQuery request, CancellationToken cancellationToken)
    {
        var invitations = await invitationRepository.GetByTenantIdAsync(userContext.TenantId, cancellationToken);
        var roles = await roleRepository.GetByTenantIdAsync(userContext.TenantId, cancellationToken);
        
        var roleDict = roles.ToDictionary(r => r.Id, r => r.Name.Value);

        // Apply search filter if provided
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            invitations = invitations.Where(i => 
                i.Email.Value.ToLower().Contains(searchTerm) ||
                roleDict.TryGetValue(i.RoleId, out var roleName) && roleName.ToLower().Contains(searchTerm));
        }

        // Apply pagination
        var totalCount = invitations.Count();
        var pagedInvitations = invitations
            .OrderByDescending(i => i.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);

        var responses = pagedInvitations.Select(invitation => new InvitationDto(
            invitation.Id,
            invitation.Email.Value,
            invitation.TenantId,
            roleDict.TryGetValue(invitation.RoleId, out var roleName) ? roleName : "Unknown",
            invitation.Token,
            invitation.InvitedBy,
            invitation.CreatedAt,
            invitation.ExpiresAt,
            invitation.IsUsed,
            invitation.UsedAt,
            invitation.UsedBy
        )).ToList();

        var pagedResult = new PagedResult<InvitationDto>(
            responses,
            request.Page,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
