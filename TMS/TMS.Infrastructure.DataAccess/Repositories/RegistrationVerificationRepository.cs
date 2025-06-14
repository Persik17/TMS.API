using Microsoft.EntityFrameworkCore;
using TMS.Abstractions.Interfaces.Repositories.BaseInterfaces;
using TMS.Infrastructure.DataAccess.Contexts;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Infrastructure.DataAccess.Repositories
{
    public class RegistrationVerificationRepository : ICommandRepository<RegistrationVerification>, IQueryRepository<RegistrationVerification>
    {
        private readonly PostgreSqlTmsContext _context;

        public RegistrationVerificationRepository(PostgreSqlTmsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<RegistrationVerification> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.RegistrationVerifications.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<RegistrationVerification>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.RegistrationVerifications.ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task InsertAsync(RegistrationVerification entity, CancellationToken cancellationToken = default)
        {
            await _context.RegistrationVerifications.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task UpdateAsync(RegistrationVerification entity, CancellationToken cancellationToken = default)
        {
            _context.RegistrationVerifications.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.RegistrationVerifications.FindAsync([entityId], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _context.RegistrationVerifications.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
