using Microsoft.EntityFrameworkCore;
using TMS.Abstractions.Interfaces.Repositories;
using TMS.Infrastructure.DataAccess.Contexts;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class CredentialRepository : ICredentialRepository<Credential>
    {
        private readonly PostgreSqlTmsContext _context;

        public CredentialRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Credential> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Credentials.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Credential>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Credentials.Where(x => x.DeleteDate == null).ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(Credential entity, CancellationToken cancellationToken = default)
        {
            await _context.Credentials.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(Credential entity, CancellationToken cancellationToken = default)
        {
            _context.Credentials.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Credentials.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                entity.DeleteDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<Credential> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Credentials.FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
        }

        public async Task<Credential> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Credentials.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
        }
    }
}
