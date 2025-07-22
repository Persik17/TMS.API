namespace TMS.API.ViewModels.Verification
{
    /// <summary>
    /// Result model for confirmation of authentication/registration.
    /// </summary>
    public class ConfirmationResultViewModel
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Error { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? UserId { get; set; }
    }
}