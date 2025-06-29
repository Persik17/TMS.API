using System.ComponentModel.DataAnnotations;
using TMS.Infrastructure.Abstractions.Contracts;

namespace TMS.Infrastructure.DataModels
{
    public class TelegramAccount : IAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public long TelegramUserId { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public long? ChatId { get; set; }
        public long AuthDate { get; set; }

        [Required]
        public string Hash { get; set; }

        public TelegramAccount() { }
    }
}
