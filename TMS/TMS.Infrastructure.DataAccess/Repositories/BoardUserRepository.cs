using Microsoft.EntityFrameworkCore;
using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Infrastructure.DataAccess.Contexts;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class BoardUserRepository : IAuditableCommandRepository<BoardUser>, IAuditableQueryRepository<BoardUser>
    {
        private readonly PostgreSqlTmsContext _context;

        public BoardUserRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<BoardUser> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.BoardUsers.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<BoardUser>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.BoardUsers.Where(x => x.DeleteDate == null).ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(BoardUser entity, CancellationToken cancellationToken = default)
        {
            await _context.BoardUsers.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(BoardUser entity, CancellationToken cancellationToken = default)
        {
            _context.BoardUsers.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.BoardUsers.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                entity.DeleteDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
