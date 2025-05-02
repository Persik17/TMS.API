using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.Contexts;
using Otus.TMS.Infrastructure.DataAccess.DataModels;
using Otus.TMS.Infrastructure.DataAccess.Interfaces;

namespace Otus.TMS.Infrastructure.DataAccess.Repositories
{
    public class CompanyRepository : IAuditableCommandRepository<Company>, IAuditableQueryRepository<Company>
    {
        private readonly PostgreSqlTmsContext _context;

        public CompanyRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Company> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Companies.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Company>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Companies.ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(Company entity, CancellationToken cancellationToken = default)
        {
            await _context.Companies.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(Company entity, CancellationToken cancellationToken = default)
        {
            _context.Companies.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Companies.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.Companies.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
