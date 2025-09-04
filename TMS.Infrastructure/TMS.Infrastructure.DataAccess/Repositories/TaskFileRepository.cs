using Microsoft.EntityFrameworkCore;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataAccess.Contexts;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class TaskFileRepository : ITaskFileRepository
    {
        private readonly PostgreSqlTmsContext _context;

        public TaskFileRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<TaskFile>> GetFilesByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default)
        {
            return await _context.TaskFiles
                .Where(f => f.TaskId == taskId && f.DeleteDate == null)
                .ToListAsync(cancellationToken);
        }

        public async Task<TaskFile?> GetByIdAsync(Guid fileId, CancellationToken cancellationToken = default)
        {
            return await _context.TaskFiles
                .FirstOrDefaultAsync(f => f.Id == fileId && f.DeleteDate == null, cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(TaskFile entity, CancellationToken cancellationToken = default)
        {
            await _context.TaskFiles.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(TaskFile entity, CancellationToken cancellationToken = default)
        {
            _context.TaskFiles.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.TaskFiles.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                entity.DeleteDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IEnumerable<TaskFile>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.TaskFiles.Where(f => f.DeleteDate == null).ToListAsync(cancellationToken);
        }
    }
}
