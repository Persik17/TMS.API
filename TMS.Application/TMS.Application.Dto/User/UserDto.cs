using TMS.Application.Dto.NotificationSetting;

namespace TMS.Application.Dto.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Guid? TelegramId { get; set; }
        public string? Timezone { get; set; }
        public string? Language { get; set; }
        public string Phone { get; set; }
        public int Status { get; set; }
        public Guid? NotificationSettingsId { get; set; }
        public NotificationSettingDto? NotificationSettings { get; set; }
        public Guid? SystemSettingsId { get; set; }
        public SystemSettingsDto? SystemSettings { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
