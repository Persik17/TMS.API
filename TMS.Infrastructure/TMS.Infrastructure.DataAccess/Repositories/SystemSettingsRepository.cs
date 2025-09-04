using Microsoft.EntityFrameworkCore;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataAccess.Contexts;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class SystemSettingsRepository : ISystemSettingsRepository
    {
        private readonly PostgreSqlTmsContext _context;
        public SystemSettingsRepository(PostgreSqlTmsContext context) => _context = context;

        public async Task<SystemSettings?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await _context.SystemSettings.FindAsync([id], cancellationToken);

        public async Task<IEnumerable<SystemSettings>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _context.SystemSettings.ToListAsync(cancellationToken);

        public async Task<SystemSettings?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
            => await _context.SystemSettings.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        public async System.Threading.Tasks.Task UpdateAsync(SystemSettings entity, CancellationToken cancellationToken = default)
        {
            _context.SystemSettings.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.SystemSettings.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.SystemSettings.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async System.Threading.Tasks.Task InsertAsync(SystemSettings entity, CancellationToken cancellationToken = default)
        {
            await _context.SystemSettings.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
