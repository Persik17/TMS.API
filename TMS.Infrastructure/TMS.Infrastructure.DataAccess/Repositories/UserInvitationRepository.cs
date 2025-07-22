using Microsoft.EntityFrameworkCore;
using TMS.Abstractions.Enums;
using TMS.Abstractions.Exceptions;
using TMS.Infrastructure.Abstractions.Repositories;
using TMS.Infrastructure.DataAccess.Contexts;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class UserInvitationRepository : IUserInvitationRepository
    {
        private readonly PostgreSqlTmsContext _context;

        public UserInvitationRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            if (entityId == Guid.Empty) throw new GuidEmptyException();

            var entity = await _context.UserInvitations.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.UserInvitations.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IEnumerable<UserInvitation>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.UserInvitations.ToListAsync(cancellationToken);
        }

        public async Task<UserInvitation?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.UserInvitations.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<UserInvitation> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty) throw new GuidEmptyException();

            return await _context.UserInvitations.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<UserInvitation?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserInvitations.FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
        }

        public async Task<IEnumerable<UserInvitation>> GetPendingInvitationsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.UserInvitations.Where(u => u.Status == (int)InvitationStatus.Pending).ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(UserInvitation entity, CancellationToken cancellationToken = default)
        {
            if (entity.Id == Guid.Empty) throw new GuidEmptyException();

            await _context.UserInvitations.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(UserInvitation entity, CancellationToken cancellationToken = default)
        {
            if (entity.Id == Guid.Empty) throw new GuidEmptyException();

            _context.UserInvitations.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
