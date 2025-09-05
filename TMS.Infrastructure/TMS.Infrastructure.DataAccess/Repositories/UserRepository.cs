using Microsoft.EntityFrameworkCore;
using TMS.Abstractions.Exceptions;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataAccess.Contexts;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PostgreSqlTmsContext _context;

        public UserRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty) throw new GuidEmptyException();

            return await _context.Users
                .Include(u => u.SystemSettings)
                .Include(n => n.NotificationSettings)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users.Where(x => x.DeleteDate == null).ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(User entity, CancellationToken cancellationToken = default)
        {
            if (entity.Id == Guid.Empty) throw new GuidEmptyException();

            await _context.Users.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
        {
            if (entity.Id == Guid.Empty) throw new GuidEmptyException();

            _context.Users.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            if (entityId == Guid.Empty) throw new GuidEmptyException();

            var entity = await _context.Users.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                entity.DeleteDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User> GetByTelegramAccountIdAsync(Guid telegramAccountId, CancellationToken cancellationToken = default)
        {
            if (telegramAccountId == Guid.Empty) throw new GuidEmptyException();

            return await _context.Users.FirstOrDefaultAsync(c => c.TelegramId == telegramAccountId, cancellationToken);
        }

        public async Task<List<User>> GetUsersByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Where(u => u.CompanyId == companyId && u.DeleteDate == null)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<User>> GetUsersByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default)
        {
            if (boardId == Guid.Empty) throw new GuidEmptyException();

            return await _context.Users
                .Include(u => u.BoardUsers)
                    .ThenInclude(bu => bu.Board)
                .Where(u => u.BoardUsers.Any(bu => bu.BoardsId == boardId) && u.DeleteDate == null)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
        {
            return await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync(cancellationToken);
        }
    }
}
