using CleanSlice.Application.Abstractions.Authentication;
using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Shared.Results;
using CleanSlice.Shared.Results.Errors;

namespace CleanSlice.Application.Features.Invitations.Commands.CancelInvitation;

internal sealed class CancelInvitationCommandHandler(
    IInvitationRepository invitationRepository,
    IUserContext userContext
    ) : ICommandHandler<CancelInvitationCommand>
{
    public async Task<Result> Handle(CancelInvitationCommand request, CancellationToken cancellationToken)
    {
        var invitation = await invitationRepository.GetByIdAsync(request.InvitationId, cancellationToken);
        
        if (invitation == null || invitation.TenantId != userContext.TenantId)
        {
            return InvitationErrors.NotFound;
        }

        if (invitation.IsUsed)
        {
            return InvitationErrors.CannotCancelUsed;
        }

        invitationRepository.Delete(invitation);

        return Result.Success();
    }
}
