using System.ComponentModel.DataAnnotations;
using TMS.Infrastructure.Abstractions.Contracts;

namespace TMS.Infrastructure.DataModels
{
    public class User : IAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        [MaxLength(255)]
        public string? FullName { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        public Guid? TelegramId { get; set; }
        public TelegramAccount? Telegram { get; set; }

        public string? Timezone { get; set; }
        public string? Language { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public Guid? NotificationSettingsId { get; set; }
        public NotificationSetting? NotificationSettings { get; set; }

        public Guid? RoleId { get; set; }
        public Role? Role { get; set; }

        public Guid? CompanyId { get; set; }
        public Company? Company { get; set; }

        public int Status { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        public List<Board> Boards { get; set; } = [];

        public User() { }
    }
}
