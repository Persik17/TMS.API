using Otus.TMS.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Otus.TMS.Infrastructure.DataAccess.DataModels
{
    public class CredentialHistory : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        public Guid CredentialId { get; set; }
        public Credential Credential { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

        public Guid ChangedById { get; set; }
        public User ChangedBy { get; set; }

        [MaxLength(50)]
        public string IPAddress { get; set; }

        public string Reason { get; set; }

        public CredentialHistory() { }
    }
}
