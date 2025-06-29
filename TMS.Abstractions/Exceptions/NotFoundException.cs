namespace TMS.Abstractions.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity is not found.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="entityType">The type of the entity that was not found.</param>
        public NotFoundException(Type entityType) : base($"Entity '{entityType.Name}' was not found.")
        {
        }
    }
}
