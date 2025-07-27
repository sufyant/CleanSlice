using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Features.Invitations.DTOs;

namespace CleanSlice.Application.Features.Invitations.Queries.GetInvitation;

public sealed record GetInvitationQuery(string Token) : IQuery<InvitationDto>;
