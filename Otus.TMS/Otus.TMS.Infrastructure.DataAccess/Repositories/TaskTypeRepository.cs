using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.Contexts;
using Otus.TMS.Infrastructure.DataAccess.DataModels;
using Otus.TMS.Infrastructure.DataAccess.Interfaces;

namespace Otus.TMS.Infrastructure.DataAccess.Repositories
{
    public class TaskTypeRepository : IAuditableCommandRepository<TaskType>, IAuditableQueryRepository<TaskType>
    {
        private readonly PostgreSqlTmsContext _context;

        public TaskTypeRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TaskType> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.TaskTypes.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<TaskType>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.TaskTypes.ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(TaskType entity, CancellationToken cancellationToken = default)
        {
            await _context.TaskTypes.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(TaskType entity, CancellationToken cancellationToken = default)
        {
            _context.TaskTypes.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.TaskTypes.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.TaskTypes.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
