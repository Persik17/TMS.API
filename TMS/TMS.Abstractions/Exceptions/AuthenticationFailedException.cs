namespace TMS.Abstractions.Exceptions
{
    /// <summary>
    /// Exception thrown when user authentication fails.
    /// </summary>
    public class AuthenticationFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationFailedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the reason for the authentication failure.</param>
        public AuthenticationFailedException(string message) : base(message)
        {
        }
    }
}
