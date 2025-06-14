namespace TMS.Abstractions.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid or empty ID is provided for a specific entity type.
    /// </summary>
    public class WrongIdException : Exception
    {
        public WrongIdException(Type entityType)
            : base($"The ID for entity '{entityType.Name}' cannot be empty or invalid.") { }
    }
}
