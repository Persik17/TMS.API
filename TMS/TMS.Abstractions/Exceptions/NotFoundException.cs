namespace TMS.Abstractions.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity is not found.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Creates a NotFoundException for a specific entity type.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        public NotFoundException(Type entityType)
            : base($"Entity '{entityType.Name}' was not found.") { }
    }
}
