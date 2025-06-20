using Microsoft.EntityFrameworkCore;
using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Infrastructure.DataAccess.Contexts;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class CredentialHistoryRepository : IAuditableCommandRepository<CredentialHistory>, IAuditableQueryRepository<CredentialHistory>
    {
        private readonly PostgreSqlTmsContext _context;

        public CredentialHistoryRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CredentialHistory> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.CredentialHistories.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<CredentialHistory>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.CredentialHistories.Where(x => x.DeleteDate == null).ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(CredentialHistory entity, CancellationToken cancellationToken = default)
        {
            await _context.CredentialHistories.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(CredentialHistory entity, CancellationToken cancellationToken = default)
        {
            _context.CredentialHistories.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.CredentialHistories.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                entity.DeleteDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
