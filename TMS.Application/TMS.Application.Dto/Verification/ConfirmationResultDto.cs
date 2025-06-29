namespace TMS.Application.Dto.Verification
{
    public class ConfirmationResultDto
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
    }
}
