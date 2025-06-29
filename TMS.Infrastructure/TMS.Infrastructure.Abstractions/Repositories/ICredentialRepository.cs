using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    /// <summary>
    /// Credential-specific repository with additional credential-related queries, extending <see cref="IAuditableCommandRepository{Credential}"/> and <see cref="IAuditableQueryRepository{Credential}"/>.
    /// </summary>
    public interface ICredentialRepository :
        IAuditableCommandRepository<Credential>,
        IAuditableQueryRepository<Credential>
    {
        /// <summary>
        /// Gets credential by user ID asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve the credential for.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous get operation. The task result contains the credential, or <c>null</c> if not found.</returns>
        Task<Credential> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets credential by login asynchronously.
        /// </summary>
        /// <param name="login">The login (e.g., username or email) to retrieve the credential for.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous get operation. The task result contains the credential, or <c>null</c> if not found.</returns>
        Task<Credential> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}