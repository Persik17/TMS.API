using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.Contexts;
using Otus.TMS.Infrastructure.DataAccess.DataModels;
using Otus.TMS.Infrastructure.DataAccess.Interfaces;

namespace Otus.TMS.Infrastructure.DataAccess.Repositories
{
    public class ColumnRepository : IAuditableCommandRepository<Column>, IAuditableQueryRepository<Column>
    {
        private readonly PostgreSqlTmsContext _context;

        public ColumnRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Column> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Columns.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Column>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Columns.ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(Column entity, CancellationToken cancellationToken = default)
        {
            await _context.Columns.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(Column entity, CancellationToken cancellationToken = default)
        {
            _context.Columns.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Columns.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.Columns.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
