namespace TMS.Abstractions.Models.Interfaces
{
    /// <summary>
    /// Represents a base entity with a unique identifier.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the entity.
        /// </summary>
        Guid Id { get; set; }
    }
}
