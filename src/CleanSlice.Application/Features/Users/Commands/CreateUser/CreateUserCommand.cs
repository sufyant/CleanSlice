using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Features.Users.DTOs;

namespace CleanSlice.Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    IEnumerable<string> Roles
    ) : ICommand<UserDto>;
