using System.ComponentModel.DataAnnotations;
using TMS.Infrastructure.Abstractions.Contracts;

namespace TMS.Infrastructure.DataModels
{
    public class RolePermission : IAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        [Required]
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        [Required]
        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; }

        public RolePermission() { }
    }
}
