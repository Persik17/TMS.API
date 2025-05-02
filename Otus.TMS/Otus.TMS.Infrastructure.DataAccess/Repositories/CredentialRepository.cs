using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.Contexts;
using Otus.TMS.Infrastructure.DataAccess.DataModels;
using Otus.TMS.Infrastructure.DataAccess.Interfaces;

namespace Otus.TMS.Infrastructure.DataAccess.Repositories
{
    public class CredentialRepository : IAuditableCommandRepository<Credential>, IAuditableQueryRepository<Credential>
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
            return await _context.Credentials.ToListAsync(cancellationToken);
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
                _context.Credentials.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
