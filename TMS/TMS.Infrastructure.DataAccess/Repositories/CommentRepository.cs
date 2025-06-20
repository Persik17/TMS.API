using Microsoft.EntityFrameworkCore;
using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Infrastructure.DataAccess.Contexts;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class CommentRepository : IAuditableCommandRepository<Comment>, IAuditableQueryRepository<Comment>
    {
        private readonly PostgreSqlTmsContext _context;

        public CommentRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Comment> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Comments.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Comment>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Comments.Where(x => x.DeleteDate == null).ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(Comment entity, CancellationToken cancellationToken = default)
        {
            await _context.Comments.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(Comment entity, CancellationToken cancellationToken = default)
        {
            _context.Comments.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Comments.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                entity.DeleteDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
