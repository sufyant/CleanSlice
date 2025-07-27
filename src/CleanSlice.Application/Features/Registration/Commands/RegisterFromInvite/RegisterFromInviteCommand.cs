using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Features.Registration.DTOs;

namespace CleanSlice.Application.Features.Registration.Commands.RegisterFromInvite;

public sealed record RegisterFromInviteCommand(
    string Token,
    string Email,
    string FirstName,
    string LastName
    ) : ICommand<RegistrationDto>;
