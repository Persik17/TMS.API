using System.ComponentModel.DataAnnotations;
using TMS.Abstractions.Models.Contracts;

namespace TMS.Infrastructure.DataAccess.DataModels
{
    public class RegistrationVerification : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Target { get; set; } // Email, phone, telegram
        public int Type { get; set; } // Храним int, работаем с enum
        public string Code { get; set; }
        public DateTime Expiration { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
    }
}