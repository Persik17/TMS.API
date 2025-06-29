namespace TMS.API.ViewModels.Registration
{
    /// <summary>
    /// Model for confirming registration via verification code.
    /// </summary>
    public class RegistrationConfirmationViewModel
    {
        public Guid VerificationId { get; set; }
        public string Code { get; set; }
    }
}