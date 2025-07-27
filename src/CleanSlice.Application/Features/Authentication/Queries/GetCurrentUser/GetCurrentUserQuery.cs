using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Features.Authentication.DTOs;

namespace CleanSlice.Application.Features.Authentication.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery : IQuery<CurrentUserDto>;
