using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Abstractions.Models.Contracts;

namespace TMS.Abstractions.Interfaces.Repositories
{
    /// <summary>
    /// Credential-specific repository with additional credential-related queries, extending <see cref="IAuditableCommandRepository{TEntity}"/> and <see cref="IAuditableQueryRepository{TEntity}"/>.
    /// </summary>
    public interface INotificationSettingRepository<TEntity> :
        ICommandRepository<TEntity>,
        IQueryRepository<TEntity>
        where TEntity : class, IEntity
    {
    }
}
