using CleanSlice.Application.Abstractions.Repositories;
using CleanSlice.Domain.Users;
using CleanSlice.Persistence.Contexts;
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
}
