using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Features.Authentication.DTOs;

namespace CleanSlice.Application.Features.Authentication.Commands.SyncUser;

public sealed record SyncUserCommand : ICommand<UserSyncDto>;
