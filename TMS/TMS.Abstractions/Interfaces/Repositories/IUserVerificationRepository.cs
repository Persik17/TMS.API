using TMS.Abstractions.Interfaces.Repositories.BaseRepositories;
using TMS.Abstractions.Models.Contracts;

namespace TMS.Abstractions.Interfaces.Repositories
{
    /// <summary>
    /// User verification-specific repository with additional verification-related queries, 
    /// extending <see cref="ICommandRepository{TEntity}"/> and <see cref="IQueryRepository{TEntity}"/>.
    /// </summary>
    public interface IUserVerificationRepository<TEntity> :
        ICommandRepository<TEntity>,
        IQueryRepository<TEntity>
        where TEntity : class, IEntity
    {
    }
}
