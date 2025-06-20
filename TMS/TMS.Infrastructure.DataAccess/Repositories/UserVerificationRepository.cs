using Microsoft.EntityFrameworkCore;
using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Infrastructure.DataAccess.Contexts;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class UserVerificationRepository : ICommandRepository<UserVerification>, IQueryRepository<UserVerification>
    {
        private readonly PostgreSqlTmsContext _context;

        public UserVerificationRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UserVerification> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.UserVerifications.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<UserVerification>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.UserVerifications.ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(UserVerification entity, CancellationToken cancellationToken = default)
        {
            await _context.UserVerifications.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(UserVerification entity, CancellationToken cancellationToken = default)
        {
            _context.UserVerifications.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.UserVerifications.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.UserVerifications.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
