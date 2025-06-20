using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Abstractions.Models.Contracts;

namespace TMS.Abstractions.Interfaces.Repositories
{
    /// <summary>
    /// User-specific repository with additional user-related queries, extending <see cref="IAuditableCommandRepository{TEntity}"/> and <see cref="IAuditableQueryRepository{TEntity}"/>.
    /// </summary>
    public interface IUserRepository<TEntity> :
        IAuditableCommandRepository<TEntity>,
        IAuditableQueryRepository<TEntity>
        where TEntity : class, IAuditableEntity
    {
        /// <summary>
        /// Finds a user by email asynchronously.
        /// </summary>
        /// <param name="email">The email address of the user to find.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous find operation. The task result contains the user, or <c>null</c> if not found.</returns>
        Task<TEntity> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}