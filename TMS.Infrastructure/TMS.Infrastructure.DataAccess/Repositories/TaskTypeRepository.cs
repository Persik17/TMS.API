using Microsoft.EntityFrameworkCore;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataAccess.Contexts;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class TaskTypeRepository : ITaskTypeRepository
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
            return await _context.TaskTypes.Where(x => x.DeleteDate == null).ToListAsync(cancellationToken);
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
                entity.DeleteDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<List<TaskType>> GetTaskTypesByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default)
        {
            return await _context.TaskTypes.Where(x => x.BoardId == boardId && x.DeleteDate == null).ToListAsync(cancellationToken);
        }
    }
}
