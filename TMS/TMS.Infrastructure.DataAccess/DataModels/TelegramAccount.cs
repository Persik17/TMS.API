using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TMS.Abstractions.Models.Contracts;

namespace TMS.Infrastructure.DataAccess.DataModels
{
    public class TelegramAccount : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        [MaxLength(255)]
        public string NickName { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        public long ChatId { get; set; }

        public TelegramAccount() { }
    }
}
