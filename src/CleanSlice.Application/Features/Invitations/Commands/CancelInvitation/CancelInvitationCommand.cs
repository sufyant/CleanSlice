using CleanSlice.Application.Abstractions.Messaging;

namespace CleanSlice.Application.Features.Invitations.Commands.CancelInvitation;

public sealed record CancelInvitationCommand(Guid InvitationId) : ICommand;
