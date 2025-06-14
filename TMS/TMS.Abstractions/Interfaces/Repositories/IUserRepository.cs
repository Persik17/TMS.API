using TMS.Abstractions.Interfaces.Repositories.BaseInterfaces;
using TMS.Abstractions.Models.Interfaces;

namespace TMS.Abstractions.Interfaces.Repositories
{
    /// <summary>
    /// User-specific repository with additional user-related queries.
    /// </summary>
    public interface IUserRepository<TEntity> :
        IAuditableCommandRepository<TEntity>,
        IAuditableQueryRepository<TEntity>
        where TEntity : class, IEntity, IAuditableEntity
    {
        /// <summary>
        /// Finds a user by email.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>User or null if not found.</returns>
        Task<TEntity> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}