using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    public interface IUserInvitationRepository :
        ICommandRepository<UserInvitation>,
        IQueryRepository<UserInvitation>
    {
        Task<UserInvitation?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<UserInvitation?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserInvitation>> GetPendingInvitationsAsync(CancellationToken cancellationToken = default);
    }
}
