using TMS.Abstractions.Interfaces.Services.BaseCommands;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for managing Company entities.
    /// Provides CRUD operations using generic read and create models.
    /// </summary>
    /// <typeparam name="TReadModel">The DTO or ViewModel used for reading and updating company data.</typeparam>
    /// <typeparam name="TCreateModel">The DTO used for creating a new company.</typeparam>
    public interface ICompanyService<TReadModel, TCreateModel> :
        ICreateService<TCreateModel, TReadModel>,
        IReadService<TReadModel>,
        IUpdateService<TReadModel>,
        IDeleteService
        where TReadModel : class
        where TCreateModel : class
    {
    }
}