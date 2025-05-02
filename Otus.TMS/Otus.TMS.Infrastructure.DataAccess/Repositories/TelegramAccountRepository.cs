using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.Contexts;
using Otus.TMS.Infrastructure.DataAccess.DataModels;
using Otus.TMS.Infrastructure.DataAccess.Interfaces;

namespace Otus.TMS.Infrastructure.DataAccess.Repositories
{
    public class TelegramAccountRepository : IAuditableCommandRepository<TelegramAccount>, IAuditableQueryRepository<TelegramAccount>
    {
        private readonly PostgreSqlTmsContext _context;

        public TelegramAccountRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TelegramAccount> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.TelegramAccounts.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<TelegramAccount>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.TelegramAccounts.ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(TelegramAccount entity, CancellationToken cancellationToken = default)
        {
            await _context.TelegramAccounts.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(TelegramAccount entity, CancellationToken cancellationToken = default)
        {
            _context.TelegramAccounts.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.TelegramAccounts.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.TelegramAccounts.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
