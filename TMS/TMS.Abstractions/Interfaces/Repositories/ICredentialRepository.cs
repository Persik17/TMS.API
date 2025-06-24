using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Abstractions.Models.Contracts;

namespace TMS.Abstractions.Interfaces.Repositories
{
    /// <summary>
    /// Credential-specific repository with additional credential-related queries, extending <see cref="IAuditableCommandRepository{TEntity}"/> and <see cref="IAuditableQueryRepository{TEntity}"/>.
    /// </summary>
    public interface ICredentialRepository<TEntity> :
        IAuditableCommandRepository<TEntity>,
        IAuditableQueryRepository<TEntity>
        where TEntity : class, IAuditableEntity
    {
        /// <summary>
        /// Gets credential by user ID asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve the credential for.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous get operation. The task result contains the credential, or <c>null</c> if not found.</returns>
        Task<TEntity> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets credential by login asynchronously.
        /// </summary>
        /// <param name="login">The login (e.g., username or email) to retrieve the credential for.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous get operation. The task result contains the credential, or <c>null</c> if not found.</returns>
        Task<TEntity> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}