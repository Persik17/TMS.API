using Otus.TMS.Domain.Models;

namespace Otus.TMS.Infrastructure.DataAccess.Contracts
{
    public interface ICommandRepository<TEntity> where TEntity : class, IEntity, IAuditableEntity
    {
        /// <summary>
        /// Добавляет новую сущность.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновляет существующую сущность.
        /// </summary>
        /// <param name="entity">Сущность для обновления.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет сущность по идентификатору.
        /// </summary>
        /// <param name="entityId">Идентификатор сущности для удаления.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default);
    }
}
