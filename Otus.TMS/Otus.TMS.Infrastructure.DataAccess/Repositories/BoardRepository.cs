using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.Contexts;
using Otus.TMS.Infrastructure.DataAccess.DataModels;
using Otus.TMS.Infrastructure.DataAccess.Interfaces;

namespace Otus.TMS.Infrastructure.DataAccess.Repositories
{
    public class BoardRepository : IAuditableCommandRepository<Board>, IAuditableQueryRepository<Board>
    {
        private readonly PostgreSqlTmsContext _context;

        public BoardRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Board> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Boards.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Board>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Boards.ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(Board entity, CancellationToken cancellationToken = default)
        {
            await _context.Boards.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(Board entity, CancellationToken cancellationToken = default)
        {
            _context.Boards.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Boards.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.Boards.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
