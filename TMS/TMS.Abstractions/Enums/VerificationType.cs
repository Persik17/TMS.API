namespace TMS.Abstractions.Enums
{
    /// <summary>
    /// Specifies the type of verification process.
    /// </summary>
    public enum VerificationType
    {
        /// <summary>
        /// Verification for authentication (e.g., login confirmation).
        /// </summary>
        Authentication = 0,

        /// <summary>
        /// Verification for new user registration.
        /// </summary>
        Registration = 1,

        /// <summary>
        /// Verification for password reset process.
        /// </summary>
        ResetPassword = 2
    }
}
