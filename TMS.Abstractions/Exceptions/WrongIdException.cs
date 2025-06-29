namespace TMS.Abstractions.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid or empty ID is provided for a specific entity type.
    /// </summary>
    public class WrongIdException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WrongIdException"/> class.
        /// </summary>
        /// <param name="entityType">The type of the entity for which the invalid ID was provided.</param>
        public WrongIdException(Type entityType) : base($"The ID for entity '{entityType.Name}' cannot be empty or invalid.")
        {
        }
    }
}
