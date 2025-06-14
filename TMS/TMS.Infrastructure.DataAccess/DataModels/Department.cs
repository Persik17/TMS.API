using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TMS.Abstractions.Models.Interfaces;

namespace TMS.Infrastructure.DataAccess.DataModels
{
    public class Department : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public Guid CompanyId { get; set; }
        public Company Company { get; set; }

        public Guid HeadId { get; set; }
        public User Head { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }

        public Department() { }
    }
}
