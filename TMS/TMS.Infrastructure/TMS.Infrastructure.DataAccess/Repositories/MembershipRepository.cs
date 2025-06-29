using Microsoft.EntityFrameworkCore;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataAccess.Contexts;
using TMS.Infrastructure.DataModels;

using Task = System.Threading.Tasks.Task;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly PostgreSqlTmsContext _context;
        public MembershipRepository(PostgreSqlTmsContext context) { _context = context; }

        public async Task InsertAsync(Membership entity, CancellationToken cancellationToken = default)
        {
            await _context.Memberships.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Membership entity, CancellationToken cancellationToken = default)
        {
            _context.Memberships.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Memberships.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.Memberships.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<Membership?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Memberships.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Membership>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Memberships
                .Where(x => x.DeleteDate == null)
                .ToListAsync(cancellationToken);
        }

        public async Task<Membership?> GetUserMembershipAsync(Guid userId, Guid resourceId, int resourceType, CancellationToken cancellationToken = default)
        {
            return await _context.Memberships
                .FirstOrDefaultAsync(m =>
                    m.UserId == userId &&
                    m.ResourceId == resourceId &&
                    m.ResourceType == resourceType &&
                    m.DeleteDate == null,
                    cancellationToken
                );
        }

        public async Task<IEnumerable<Membership>> GetUserMembershipsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Memberships
                .Where(m => m.UserId == userId && m.DeleteDate == null)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Membership>> GetMembershipsForResourceAsync(Guid resourceId, int resourceType, CancellationToken cancellationToken = default)
        {
            return await _context.Memberships
                .Where(m => m.ResourceId == resourceId && m.ResourceType == resourceType && m.DeleteDate == null)
                .ToListAsync(cancellationToken);
        }
    }
}
