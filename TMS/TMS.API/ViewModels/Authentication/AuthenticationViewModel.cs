namespace TMS.API.ViewModels.Authentication
{
    /// <summary>
    /// Model for user authentication via email/password.
    /// </summary>
    public class AuthenticationViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}