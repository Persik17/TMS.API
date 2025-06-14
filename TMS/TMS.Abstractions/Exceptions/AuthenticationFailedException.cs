namespace TMS.Abstractions.Exceptions
{
    /// <summary>
    /// Exception thrown when user authentication fails.
    /// </summary>
    public class AuthenticationFailedException : Exception
    {
        public AuthenticationFailedException(string message) : base(message) { }
    }
}
