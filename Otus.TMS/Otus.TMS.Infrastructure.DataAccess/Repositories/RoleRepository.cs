using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.Contexts;
using Otus.TMS.Infrastructure.DataAccess.DataModels;
using Otus.TMS.Infrastructure.DataAccess.Interfaces;

namespace Otus.TMS.Infrastructure.DataAccess.Repositories
{
    public class RoleRepository : IAuditableCommandRepository<Role>, IAuditableQueryRepository<Role>
    {
        private readonly PostgreSqlTmsContext _context;

        public RoleRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Role> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Roles.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Roles.ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(Role entity, CancellationToken cancellationToken = default)
        {
            await _context.Roles.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(Role entity, CancellationToken cancellationToken = default)
        {
            _context.Roles.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Roles.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.Roles.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
