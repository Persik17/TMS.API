using System.ComponentModel.DataAnnotations;
using TMS.Infrastructure.Abstractions.Contracts;

namespace TMS.Infrastructure.DataModels
{
    public class CredentialHistory : IAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        [Required]
        public Guid CredentialId { get; set; }
        public Credential Credential { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

        [Required]
        public Guid ChangedById { get; set; }
        public User ChangedBy { get; set; }

        [Required]
        [MaxLength(50)]
        public string IPAddress { get; set; }

        public string? Reason { get; set; }

        public CredentialHistory() { }
    }
}
