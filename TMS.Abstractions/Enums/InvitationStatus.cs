namespace TMS.Abstractions.Enums
{
    /// <summary>
    /// Specifies the status of an invitation.
    /// </summary>
    public enum InvitationStatus
    {
        /// <summary>
        /// The invitation is pending and awaiting a response.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// The invitation has been accepted.
        /// </summary>
        Accepted = 1,

        /// <summary>
        /// The invitation has expired and is no longer valid.
        /// </summary>
        Expired = 2,

        /// <summary>
        /// The invitation has been cancelled.
        /// </summary>
        Cancelled = 3
    }
}