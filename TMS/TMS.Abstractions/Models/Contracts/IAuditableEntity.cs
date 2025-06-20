namespace TMS.Abstractions.Models.Contracts
{
    /// <summary>
    /// Represents an entity with audit information (creation, update, and deletion timestamps).  Inherits from <see cref="IEntity"/>.
    /// </summary>
    public interface IAuditableEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the date and time when the entity was created.
        /// </summary>
        DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was last updated.  This property is nullable.
        /// </summary>
        DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was deleted. This property is nullable.
        /// </summary>
        DateTime? DeleteDate { get; set; }
    }
}
