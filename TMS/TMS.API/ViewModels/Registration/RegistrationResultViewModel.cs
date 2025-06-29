namespace TMS.API.ViewModels.Registration
{
    /// <summary>
    /// Result model for registration requests.
    /// </summary>
    public class RegistrationResultViewModel
    {
        public bool Success { get; set; }
        public Guid VerificationId { get; set; }
        public DateTime Expiration { get; set; }
        public string Error { get; set; }
    }
}