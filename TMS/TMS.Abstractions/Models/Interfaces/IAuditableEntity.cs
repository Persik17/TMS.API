namespace TMS.Abstractions.Models.Interfaces
{
    /// <summary>
    /// Represents an entity with audit information (creation, update, deletion dates).
    /// </summary>
    public interface IAuditableEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the creation date of the entity.
        /// </summary>
        DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the last update date of the entity.
        /// </summary>
        DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the deletion date of the entity.
        /// </summary>
        DateTime? DeleteDate { get; set; }
    }
}
