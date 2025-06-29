namespace TMS.Abstractions.Exceptions
{
    public class ForbiddenException : Exception
    {
        public string MissingPermission { get; }

        public ForbiddenException(string missingPermission)
            : base($"Access denied. Missing permission: {missingPermission}")
        {
            MissingPermission = missingPermission;
        }

        public ForbiddenException(string message, string missingPermission)
            : base(message)
        {
            MissingPermission = missingPermission;
        }
    }
}
