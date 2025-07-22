namespace TMS.Application.Dto.User
{
    public class UserInviteDto
    {
        public string Email { get; set; }
        public string? FullName { get; set; }
        public string[] Roles { get; set; }
        public string? Language { get; set; }
        public string? CustomMessage { get; set; }
    }
}
