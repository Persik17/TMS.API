namespace TMS.Application.Models.DTOs.Authentication
{
    public class AuthenticationResultDto
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Error { get; set; }
    }
}
