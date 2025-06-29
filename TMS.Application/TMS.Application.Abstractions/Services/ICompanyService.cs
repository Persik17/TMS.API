using TMS.Application.Abstractions.Services.BaseCommands;
using TMS.Application.Dto.Company;

namespace TMS.Application.Abstractions.Services
{
    /// <summary>
    /// Service contract for managing Company entities.
    /// Provides CRUD operations using generic read and create models.
    /// </summary>
    public interface ICompanyService :
        ICreateService<CompanyCreateDto, CompanyDto>,
        IReadService<CompanyDto>,
        IUpdateService<CompanyDto>,
        IDeleteService
    {
    }
}