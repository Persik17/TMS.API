using Microsoft.EntityFrameworkCore;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataAccess.Contexts;
using Task = TMS.Infrastructure.DataModels.Task;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class TaskRepository : ITaskRepository
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
            return await _context.Tasks.Where(x => x.DeleteDate == null).ToListAsync(cancellationToken);
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
                entity.DeleteDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<List<Task>> GetTasksByColumnIdAsync(Guid columnId, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks.Where(x => x.ColumnId == columnId && x.DeleteDate == null).ToListAsync(cancellationToken);
        }

        public async Task<List<Task>> GetTasksByColumnIdsAsync(IEnumerable<Guid> columnIds, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks.Where(x => columnIds.Contains(x.ColumnId) && x.DeleteDate == null).ToListAsync(cancellationToken);
        }

        public async Task<List<Task>> GetTasksByAssigneeIdAsync(Guid assigneeId, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .Where(x => x.AssigneeId == assigneeId && x.DeleteDate == null)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Task>> SearchTasksByBoardIdsAsync(string query, List<Guid> boardIds, CancellationToken cancellationToken = default)
        {
            if (boardIds == null || boardIds.Count == 0)
                return new List<Task>();

            return await _context.Tasks
                .Where(t => boardIds.Contains(t.BoardId) && t.Name.Contains(query) && t.DeleteDate == null)
                .ToListAsync(cancellationToken);
        }
    }
}
