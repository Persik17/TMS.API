using TMS.Abstractions.Interfaces.Repositories.BaseInterfaces;
using TMS.Abstractions.Models.Interfaces;

namespace TMS.Abstractions.Interfaces.Repositories
{
    /// <summary>
    /// Credential-specific repository with additional credential-related queries.
    /// </summary>
    public interface ICredentialRepository<TEntity> :
        IAuditableCommandRepository<TEntity>,
        IAuditableQueryRepository<TEntity>
        where TEntity : class, IEntity, IAuditableEntity
    {
        /// <summary>
        /// Gets credential by user id.
        /// </summary>
        Task<TEntity> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}