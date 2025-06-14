namespace TMS.API.ViewModels.Registration
{
    public class RegistrationViewModel
    {
        public string Target { get; set; }
        public int Type { get; set; } // Email/Phone/Telegram
        public string Password { get; set; }
    }
}