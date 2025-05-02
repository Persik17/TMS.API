using Microsoft.EntityFrameworkCore;
using Otus.TMS.Infrastructure.DataAccess.Contexts;
using Otus.TMS.Infrastructure.DataAccess.DataModels;
using Otus.TMS.Infrastructure.DataAccess.Interfaces;

namespace Otus.TMS.Infrastructure.DataAccess.Repositories
{
    public class DepartmentRepository : IAuditableCommandRepository<Department>, IAuditableQueryRepository<Department>
    {
        private readonly PostgreSqlTmsContext _context;

        public DepartmentRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Department> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Departments.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Departments.ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(Department entity, CancellationToken cancellationToken = default)
        {
            await _context.Departments.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(Department entity, CancellationToken cancellationToken = default)
        {
            _context.Departments.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Departments.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.Departments.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
