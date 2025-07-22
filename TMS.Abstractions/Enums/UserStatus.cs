namespace TMS.Abstractions.Enums
{
    /// <summary>
    /// Specifies the status of a user in the system.
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// The user account is pending activation or approval.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// The user account is active and has full access.
        /// </summary>
        Active = 1,

        /// <summary>
        /// The user account is blocked and access is restricted.
        /// </summary>
        Blocked = 2,

        /// <summary>
        /// The user has been invited but has not yet completed registration or activation.
        /// </summary>
        Invited = 3
    }
}