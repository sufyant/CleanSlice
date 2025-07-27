using CleanSlice.Application.Abstractions.Authentication;
using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Application.Features.Authentication.DTOs;
using CleanSlice.Shared.Results;
using CleanSlice.Shared.Results.Errors;

namespace CleanSlice.Application.Features.Authentication.Queries.GetCurrentUser;

internal sealed class GetCurrentUserQueryHandler(
    IUserRepository userRepository,
    IUserContext userContext) : IQueryHandler<GetCurrentUserQuery, CurrentUserDto>
{
    public async Task<Result<CurrentUserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var identityId = userContext.IdentityId;
        var user = await userRepository.GetByIdentityIdAsync(identityId, cancellationToken);
        
        if (user == null)
        {
            return UserErrors.NotFound;
        }

        var response = new CurrentUserDto(
            user.Id,
            user.IdentityId,
            user.TenantId,
            user.Email.Value,
            user.FirstName,
            user.LastName,
            user.FullName,
            user.IsActive,
            userContext.Roles.ToList(),
            userContext.Permissions.ToList()
        );

        return Result.Success(response);
    }
}
