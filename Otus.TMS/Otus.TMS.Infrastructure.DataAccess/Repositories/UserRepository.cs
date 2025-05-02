using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.Contexts;
using Otus.TMS.Infrastructure.DataAccess.DataModels;
using Otus.TMS.Infrastructure.DataAccess.Interfaces;

namespace Otus.TMS.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IAuditableCommandRepository<User>, IAuditableQueryRepository<User>
    {
        private readonly PostgreSqlTmsContext _context;

        public UserRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users.ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(User entity, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Users.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.Users.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
