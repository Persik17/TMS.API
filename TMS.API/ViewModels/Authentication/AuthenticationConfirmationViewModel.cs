namespace TMS.API.ViewModels.Authentication
{
    /// <summary>
    /// Model for confirming user authentication with a verification code.
    /// </summary>
    public class AuthenticationConfirmationViewModel
    {
        public Guid VerificationId { get; set; }
        public string Code { get; set; }
    }
}
