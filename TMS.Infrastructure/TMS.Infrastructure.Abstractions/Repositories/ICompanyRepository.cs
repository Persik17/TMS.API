using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    public interface ICompanyRepository :
        IAuditableCommandRepository<Company>,
        IAuditableQueryRepository<Company>
    {
        Task<Company?> GetCompanyByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
