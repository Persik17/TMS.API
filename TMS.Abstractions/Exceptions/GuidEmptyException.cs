namespace TMS.Abstractions.Exceptions
{
    /// <summary>
    /// Exception thrown when a provided Guid value is empty.
    /// </summary>
    public class GuidEmptyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuidEmptyException"/> class with a default error message.
        /// </summary>
        public GuidEmptyException() : base("Guid must not be empty.")
        {
        }
    }
}