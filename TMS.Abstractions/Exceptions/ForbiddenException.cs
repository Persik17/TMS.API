namespace TMS.Abstractions.Exceptions
{
    /// <summary>
    /// Exception thrown when a user attempts to access a resource without the required permissions.
    /// </summary>
    public class ForbiddenException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class with a default error message.
        /// </summary>
        public ForbiddenException()
            : base("Access denied. Missing permission")
        {
        }
    }
}