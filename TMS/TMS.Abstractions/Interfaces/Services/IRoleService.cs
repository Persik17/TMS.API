using TMS.Abstractions.Interfaces.Services.BaseCommands;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Service contract for managing Role entities.
    /// Provides CRUD operations.
    /// </summary>
    /// <typeparam name="TReadModel">DTO used for reading roles.</typeparam>
    /// <typeparam name="TCreateModel">DTO used for creating roles.</typeparam>
    public interface IRoleService<TReadModel, TCreateModel> :
        ICreateService<TCreateModel, TReadModel>,
        IReadService<TReadModel>,
        IUpdateService<TReadModel>,
        IDeleteService
        where TReadModel : class
        where TCreateModel : class
    {
    }
}