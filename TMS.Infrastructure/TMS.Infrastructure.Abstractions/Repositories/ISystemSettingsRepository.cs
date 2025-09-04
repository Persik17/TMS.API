using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    public interface ISystemSettingsRepository : IQueryRepository<SystemSettings>, ICommandRepository<SystemSettings>
    {
        Task<SystemSettings?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
