using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.Contexts;
using Otus.TMS.Infrastructure.DataAccess.DataModels;
using Otus.TMS.Infrastructure.DataAccess.Interfaces;

namespace Otus.TMS.Infrastructure.DataAccess.Repositories
{
    public class PermissionRepository : IAuditableCommandRepository<Permission>, IAuditableQueryRepository<Permission>
    {
        private readonly PostgreSqlTmsContext _context;

        public PermissionRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Permission> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Permissions.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Permissions.ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(Permission entity, CancellationToken cancellationToken = default)
        {
            await _context.Permissions.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(Permission entity, CancellationToken cancellationToken = default)
        {
            _context.Permissions.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Permissions.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.Permissions.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
