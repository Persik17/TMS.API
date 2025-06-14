namespace TMS.API.ViewModels.Authentication
{
    public class AuthenticationResultViewModel
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Error { get; set; }
    }
}