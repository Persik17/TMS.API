using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    /// <summary>
    /// User verification-specific repository with additional verification-related queries, 
    /// extending <see cref="ICommandRepository{UserVerification}"/> and <see cref="IQueryRepository{UserVerification}"/>.
    /// </summary>
    public interface IUserVerificationRepository :
        ICommandRepository<UserVerification>,
        IQueryRepository<UserVerification>
    {
    }
}
