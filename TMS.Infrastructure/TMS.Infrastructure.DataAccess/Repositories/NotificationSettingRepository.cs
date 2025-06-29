using Microsoft.EntityFrameworkCore;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataAccess.Contexts;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class NotificationSettingRepository : INotificationSettingRepository
    {
        private readonly PostgreSqlTmsContext _context;

        public NotificationSettingRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<NotificationSetting> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.NotificationSettings.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<NotificationSetting>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.NotificationSettings.ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(NotificationSetting entity, CancellationToken cancellationToken = default)
        {
            await _context.NotificationSettings.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(NotificationSetting entity, CancellationToken cancellationToken = default)
        {
            _context.NotificationSettings.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.NotificationSettings.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.NotificationSettings.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
