using Microsoft.EntityFrameworkCore;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataAccess.Contexts;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class BoardRepository : IBoardRepository
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
            return await _context.Boards.Where(x => x.DeleteDate == null).ToListAsync(cancellationToken);
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
                entity.DeleteDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<List<Board>> GetBoardsByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default)
        {
            return await _context.Boards.Where(x => x.CompanyId == companyId && x.DeleteDate == null).ToListAsync(cancellationToken);
        }
    }
}
