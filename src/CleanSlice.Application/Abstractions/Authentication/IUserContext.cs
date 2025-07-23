namespace CleanSlice.Application.Abstractions.Authentication;

public interface IUserContext
{
    Guid UserId { get; }
    Guid TenantId { get; }
    string IdentityId { get; }
    string Email { get; }
    string Name { get; }
}
