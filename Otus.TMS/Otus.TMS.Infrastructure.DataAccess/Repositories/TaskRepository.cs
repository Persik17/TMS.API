using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.Contexts;
using Otus.TMS.Infrastructure.DataAccess.Interfaces;

using Task = Otus.TMS.Infrastructure.DataAccess.DataModels.Task;

namespace Otus.TMS.Infrastructure.DataAccess.Repositories
{
    public class TaskRepository : IAuditableCommandRepository<Task>, IAuditableQueryRepository<Task>
    {
        private readonly PostgreSqlTmsContext _context;

        public TaskRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Task> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Task>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Tasks.ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(Task entity, CancellationToken cancellationToken = default)
        {
            await _context.Tasks.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(Task entity, CancellationToken cancellationToken = default)
        {
            _context.Tasks.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Tasks.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.Tasks.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
