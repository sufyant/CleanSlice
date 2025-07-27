using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Features.Invitations.DTOs;

namespace CleanSlice.Application.Features.Invitations.Commands.CreateInvitation;

public sealed record CreateInvitationCommand(
    string Email,
    string RoleName,
    int? ExpirationDays = 7
    ) : ICommand<InvitationDto>;
