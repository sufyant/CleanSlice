using CleanSlice.Application.Abstractions.Authentication;
using CleanSlice.Application.Abstractions.Messaging;
using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Application.Features.Users.DTOs;
using CleanSlice.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Application.Features.Users.Queries.GetUsers;

internal sealed class GetUsersQueryHandler(
    IUserRepository userRepository,
    IUserContext userContext) : IQueryHandler<GetUsersQuery, PagedResult<UserDto>>
{
    public async Task<Result<PagedResult<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = userRepository.Query()
            .Where(u => u.TenantId == userContext.TenantId);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(u => 
                u.Email.Value.ToLower().Contains(searchTerm) ||
                u.FirstName.ToLower().Contains(searchTerm) ||
                u.LastName.ToLower().Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        var users = await query
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new UserDto(
                u.Id,
                u.IdentityId,
                u.TenantId,
                u.Email,
                u.FirstName,
                u.LastName,
                u.FullName,
                u.IsActive,
                u.UserRoles.Select(ur => ur.Role.Name.Value)))
            .ToListAsync(cancellationToken);

        var pagedResult = new PagedResult<UserDto>(
            users,
            request.Page,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
