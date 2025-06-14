using Microsoft.EntityFrameworkCore;
using TMS.Abstractions.Interfaces.Repositories.BaseInterfaces;
using TMS.Infrastructure.DataAccess.Contexts;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class UserDepartmentRepository : IAuditableCommandRepository<UserDepartment>, IAuditableQueryRepository<UserDepartment>
    {
        private readonly PostgreSqlTmsContext _context;

        public UserDepartmentRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UserDepartment> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.UserDepartments.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<UserDepartment>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.UserDepartments.Where(x => x.DeleteDate == null).ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(UserDepartment entity, CancellationToken cancellationToken = default)
        {
            await _context.UserDepartments.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(UserDepartment entity, CancellationToken cancellationToken = default)
        {
            _context.UserDepartments.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.UserDepartments.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                entity.DeleteDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
