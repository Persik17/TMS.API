using TMS.Abstractions.Interfaces.Services.BaseCommands;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for managing Department entities.
    /// Provides CRUD operations using generic read and create models.
    /// </summary>
    /// <typeparam name="TReadModel">The DTO or ViewModel used for reading and updating department data.</typeparam>
    /// <typeparam name="TCreateModel">The DTO used for creating a new department.</typeparam>
    public interface IDepartmentService<TReadModel, TCreateModel> :
        ICreateService<TCreateModel, TReadModel>,
        IReadService<TReadModel>,
        IUpdateService<TReadModel>,
        IDeleteService
        where TReadModel : class
        where TCreateModel : class
    {
    }
}