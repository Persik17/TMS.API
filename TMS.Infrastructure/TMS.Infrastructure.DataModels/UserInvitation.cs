using System.ComponentModel.DataAnnotations;
using TMS.Infrastructure.Abstractions.Contracts;

namespace TMS.Infrastructure.DataModels
{
    public class UserInvitation : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string[]? Roles { get; set; }
        public string? Language { get; set; }
        [Required]
        public Guid InvitedBy { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime Expiration { get; set; }
        [Required]
        public int Status { get; set; }

        [MaxLength(300)]
        public string? CustomMessage { get; set; }
    }
}
