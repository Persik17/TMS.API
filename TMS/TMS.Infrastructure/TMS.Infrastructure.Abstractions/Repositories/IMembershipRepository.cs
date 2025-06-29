using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    public interface IMembershipRepository :
        IAuditableCommandRepository<Membership>,
         IAuditableQueryRepository<Membership>
    {
        Task<Membership?> GetUserMembershipAsync(Guid userId, Guid resourceId, int resourceType, CancellationToken cancellationToken = default);
        Task<IEnumerable<Membership>> GetUserMembershipsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Membership>> GetMembershipsForResourceAsync(Guid resourceId, int resourceType, CancellationToken cancellationToken = default);
    }
}