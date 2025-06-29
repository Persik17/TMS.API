namespace TMS.Infrastructure.Abstractions.Contracts
{
    /// <summary>
    /// Represents a base entity with a unique identifier of type <see cref="Guid"/>.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the entity.
        /// </summary>
        Guid Id { get; set; }
    }
}
