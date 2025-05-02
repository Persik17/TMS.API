using Otus.TMS.Domain.Models;

namespace Otus.TMS.Infrastructure.DataAccess.Interfaces
{
    public interface IQueryRepository<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Получает сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Сущность или null, если не найдена.</returns>
        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает все сущности.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Список сущностей.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    }

    public interface IAuditableQueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : class, IEntity, IAuditableEntity { }
}
