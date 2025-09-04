using TMS.Abstractions.Enums;

namespace TMS.Application.Dto.User
{
    public class SystemSettingsDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Theme { get; set; }
        public string BoardBackgroundUrl { get; set; }
        public string BoardBackgroundName { get; set; }
    }
}
