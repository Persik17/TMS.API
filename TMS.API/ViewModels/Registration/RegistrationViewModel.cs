namespace TMS.API.ViewModels.Registration
{
    /// <summary>
    /// Model for user registration request (email and password).
    /// </summary>
    public class RegistrationViewModel
    {
        /// <summary>
        /// User's email for registration.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password for registration.
        /// </summary>
        public string Password { get; set; }
    }
}