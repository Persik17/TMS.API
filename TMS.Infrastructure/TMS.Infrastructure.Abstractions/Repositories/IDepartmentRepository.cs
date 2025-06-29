using TMS.Infrastructure.Abstractions.Repositories.BaseRepositories;
using TMS.Infrastructure.DataModels;

namespace TMS.Infrastructure.Abstractions.Repositories
{
    /// <summary>
    /// Board-specific repository with additional board-related queries, extending <see cref="IAuditableCommandRepository{Department}"/> and <see cref="IAuditableQueryRepository{Department}"/>.
    /// </summary>
    public interface IDepartmentRepository :
        IAuditableCommandRepository<Department>,
        IAuditableQueryRepository<Department>
    {
        Task<List<Department>> GetDepartmentsByCompanyId(Guid companyId, CancellationToken cancellationToken = default);
    }
}