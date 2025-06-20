namespace TMS.Abstractions.Models.DTOs.Authentication
{
    /// <summary>
    /// Represents the data transfer object (DTO) for authentication credentials.
    /// </summary>
    public class AuthenticationDto
    {
        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        public string Password { get; set; }
    }
}