using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Contexts;
using CleanSlice.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace CleanSlice.Persistence.Repositories;

internal sealed class InvitationRepository(ApplicationDbContext dbContext) : BaseRepository<Invitation>(dbContext), IInvitationRepository
{
    public async Task<Invitation?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await dbContext.Invitations
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Token == token, cancellationToken);
    }

    public async Task<Invitation?> GetPendingByEmailAsync(string email, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Invitations
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Email.Value == email &&
                                    i.TenantId == tenantId &&
                                    !i.IsUsed &&
                                    !i.IsExpired, cancellationToken);
    }

    public async Task<IEnumerable<Invitation>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Invitations
            .AsNoTracking()
            .Where(i => i.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Invitation>> GetExpiredInvitationsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Invitations
            .AsNoTracking()
            .Where(i => i.IsExpired)
            .ToListAsync(cancellationToken);
    }

    // Pagination methods
    public async Task<PagedResult<Invitation>> GetPagedInvitationsAsync(Guid tenantId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Invitations
            .AsNoTracking()
            .Where(i => i.TenantId == tenantId);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(i =>
                i.Email.Value.Contains(request.SearchTerm) ||
                i.Token.Contains(request.SearchTerm));
        }

        // Apply default sorting if not specified
        if (string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = query.OrderByDescending(i => i.CreatedAt);
        }

        return await query.ToPagedResultAsync(request, cancellationToken);
    }

    public async Task<PagedResult<Invitation>> GetPagedPendingInvitationsAsync(Guid tenantId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Invitations
            .AsNoTracking()
            .Where(i => i.TenantId == tenantId && !i.IsUsed && !i.IsExpired);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(i =>
                i.Email.Value.Contains(request.SearchTerm) ||
                i.Token.Contains(request.SearchTerm));
        }

        // Apply default sorting if not specified
        if (string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = query.OrderByDescending(i => i.CreatedAt);
        }

        return await query.ToPagedResultAsync(request, cancellationToken);
    }

    public async Task<PagedResult<Invitation>> GetPagedExpiredInvitationsAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Invitations
            .AsNoTracking()
            .Where(i => i.IsExpired);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(i =>
                i.Email.Value.Contains(request.SearchTerm) ||
                i.Token.Contains(request.SearchTerm));
        }

        // Apply default sorting if not specified
        if (string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = query.OrderByDescending(i => i.ExpiresAt);
        }

        return await query.ToPagedResultAsync(request, cancellationToken);
    }
}
