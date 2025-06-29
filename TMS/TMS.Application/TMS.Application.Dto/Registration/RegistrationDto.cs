namespace TMS.Application.Dto.Registration
{
    /// <summary>
    /// Represents the data transfer object (DTO) for user registration.
    /// </summary>
    public class RegistrationDto
    {
        /// <summary>
        /// Gets or sets the email for registration.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's desired password.
        /// </summary>
        public string Password { get; set; }
    }
}