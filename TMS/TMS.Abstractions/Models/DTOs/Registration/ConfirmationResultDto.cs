namespace TMS.Abstractions.Models.DTOs.Registration
{
    /// <summary>
    /// Represents the data transfer object (DTO) for the result of a confirmation operation (e.g., email confirmation).
    /// </summary>
    public class ConfirmationResultDto
    {
        /// <summary>
        /// Gets or sets a value indicating whether the confirmation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error message if the confirmation failed. This property will be <c>null</c> if the confirmation was successful.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the authentication token issued upon successful confirmation. This property will be <c>null</c> if confirmation failed or a token is not issued.
        /// </summary>
        public string Token { get; set; }
    }
}
