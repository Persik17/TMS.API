using TMS.Abstractions.Interfaces.Services.BaseCommands;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for managing Permission entities.
    /// Provides create, update, and get operations.
    /// </summary>
    /// <typeparam name="TReadModel">DTO used for reading permissions.</typeparam>
    /// <typeparam name="TCreateModel">DTO used for creating permissions.</typeparam>
    public interface IPermissionService<TReadModel, TCreateModel> :
        ICreateService<TCreateModel, TReadModel>,
        IReadService<TReadModel>,
        IUpdateService<TReadModel>,
        IDeleteService
        where TReadModel : class
        where TCreateModel : class
    {
    }
}