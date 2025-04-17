using Otus.TMS.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Otus.TMS.Infrastructure.DataAccess.DataModels
{
    public class User : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        public Guid? TelegramId { get; set; }
        public TelegramAccount Telegram { get; set; }

        [MaxLength(50)]
        public string Timezone { get; set; }

        [MaxLength(10)]
        public string Language { get; set; }

        public string RegistrationDate { get; set; }

        public string LastLoginDate { get; set; }


        public Guid NotificationSettingsId { get; set; }
        public NotificationSetting NotificationSettings { get; set; }

        public int Status { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        public List<BoardUser> BoardUsers { get; set; } = [];
        public List<BoardUserRole> BoardUserRoles { get; set; } = [];

        public User() { }
    }
}
