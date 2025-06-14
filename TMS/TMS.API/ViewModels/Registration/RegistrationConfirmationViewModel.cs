namespace TMS.API.ViewModels.Registration
{
    public class RegistrationConfirmationViewModel
    {
        public Guid VerificationId { get; set; }
        public string Code { get; set; }
    }
}