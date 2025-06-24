namespace TMS.API.ViewModels.Registration
{
    public class AuthenticationConfirmationViewModel
    {
        public Guid VerificationId { get; set; }
        public string Code { get; set; }
    }
}