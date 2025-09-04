using System.ComponentModel.DataAnnotations;
using TMS.Infrastructure.Abstractions.Contracts;

namespace TMS.Infrastructure.DataModels
{
    public class NotificationSetting : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public bool EmailNotificationsEnabled { get; set; }

        [Required]
        public bool PushNotificationsEnabled { get; set; }

        [Required]
        public bool TelegramNotificationsEnabled { get; set; }

        public NotificationSetting() { }
    }
}
