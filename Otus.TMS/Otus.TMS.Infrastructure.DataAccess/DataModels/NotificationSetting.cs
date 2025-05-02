using Otus.TMS.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Otus.TMS.Infrastructure.DataAccess.DataModels
{
    public class NotificationSetting : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public bool EmailNotificationsEnabled { get; set; }

        public bool PushNotificationsEnabled { get; set; }

        public bool TelegramNotificationsEnabled { get; set; }

        public NotificationSetting() { }
    }
}
