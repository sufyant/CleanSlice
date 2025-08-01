using CleanSlice.Domain.Users;
using CleanSlice.Shared.Results;

namespace CleanSlice.Application.Abstractions.Repositories;

public interface IInvitationRepository : IBaseRepository<Invitation>
{
    Task<Invitation?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<Invitation?> GetPendingByEmailAsync(string email, Guid tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Invitation>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Invitation>> GetExpiredInvitationsAsync(CancellationToken cancellationToken = default);

    // Pagination methods
    Task<PagedResult<Invitation>> GetPagedInvitationsAsync(Guid tenantId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<Invitation>> GetPagedPendingInvitationsAsync(Guid tenantId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<Invitation>> GetPagedExpiredInvitationsAsync(PagedRequest request, CancellationToken cancellationToken = default);
}
