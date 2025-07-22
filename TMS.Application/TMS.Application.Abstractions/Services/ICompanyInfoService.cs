using TMS.Application.Dto.Company;

namespace TMS.Application.Abstractions.Services
{
    public interface ICompanyInfoService
    {
        Task<CompanyInfoDto?> GetCompanyInfoByUserId(Guid userId, CancellationToken cancellationToken = default);
        Task GetTarifInfoByCompanyId(Guid companyId, Guid userId, CancellationToken cancellationToken = default);
        Task GetCEOInfoByCompanyId(Guid companyId, Guid userId, CancellationToken cancellationToken = default);
    }
}
