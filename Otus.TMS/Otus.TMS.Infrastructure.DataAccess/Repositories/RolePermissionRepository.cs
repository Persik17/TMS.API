using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.Contexts;
using Otus.TMS.Infrastructure.DataAccess.DataModels;
using Otus.TMS.Infrastructure.DataAccess.Interfaces;

namespace Otus.TMS.Infrastructure.DataAccess.Repositories
{
    public class RolePermissionRepository : IAuditableCommandRepository<RolePermission>, IAuditableQueryRepository<RolePermission>
    {
        private readonly PostgreSqlTmsContext _context;

        public RolePermissionRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<RolePermission> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.RolePermissions.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<RolePermission>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.RolePermissions.ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(RolePermission entity, CancellationToken cancellationToken = default)
        {
            await _context.RolePermissions.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(RolePermission entity, CancellationToken cancellationToken = default)
        {
            _context.RolePermissions.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.RolePermissions.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.RolePermissions.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
