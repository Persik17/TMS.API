namespace TMS.Abstractions.Models.Models
{
    /// <summary>
    /// Represents the data required for initiating a verification process.
    /// </summary>
    public class VerificationData
    {
        /// <summary>
        /// Gets or sets the target for verification (e.g., email address or phone number).
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the type of verification or notification (represented as an integer).
        /// </summary>
        public int Type { get; set; }
    }
}
