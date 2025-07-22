using System.ComponentModel.DataAnnotations;
using TMS.Infrastructure.Abstractions.Contracts;

namespace TMS.Infrastructure.DataModels
{
    public class Role : IAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public List<User> Users { get; set; } = [];

        public Role() { }
    }
}
