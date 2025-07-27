using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Features.Registration.DTOs;

namespace CleanSlice.Application.Features.Registration.Queries.ResolveInvitation;

public sealed record ResolveInvitationQuery(string Token) : IQuery<InvitationDetailsDto>;
