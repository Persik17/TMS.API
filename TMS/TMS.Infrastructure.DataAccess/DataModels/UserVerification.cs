using System.ComponentModel.DataAnnotations;
using TMS.Abstractions.Models.Contracts;

namespace TMS.Infrastructure.DataAccess.DataModels
{
    public class UserVerification : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime Expiration { get; set; }
        public bool IsUsed { get; set; }
        public int Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }

        public User User { get; set; }
    }
}