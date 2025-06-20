namespace TMS.Abstractions.Models.DTOs.Authentication
{
    /// <summary>
    /// Represents the data transfer object (DTO) for the result of an authentication attempt.
    /// </summary>
    public class AuthenticationResultDto
    {
        /// <summary>
        /// Gets or sets a value indicating whether the authentication was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the authentication token if the authentication was successful.  This property will be <c>null</c> if authentication failed.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the error message if the authentication failed. This property will be <c>null</c> if authentication was successful.
        /// </summary>
        public string Error { get; set; }
    }
}
