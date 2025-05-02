using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.Contexts;
using Otus.TMS.Infrastructure.DataAccess.DataModels;
using Otus.TMS.Infrastructure.DataAccess.Interfaces;

namespace Otus.TMS.Infrastructure.DataAccess.Repositories
{
    public class BoardUserRoleRepository : IAuditableCommandRepository<BoardUserRole>, IAuditableQueryRepository<BoardUserRole>
    {
        private readonly PostgreSqlTmsContext _context;

        public BoardUserRoleRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<BoardUserRole> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.BoardUserRoles.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<BoardUserRole>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.BoardUserRoles.ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(BoardUserRole entity, CancellationToken cancellationToken = default)
        {
            await _context.BoardUserRoles.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(BoardUserRole entity, CancellationToken cancellationToken = default)
        {
            _context.BoardUserRoles.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.BoardUserRoles.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.BoardUserRoles.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
