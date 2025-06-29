using System.ComponentModel.DataAnnotations;
using TMS.Infrastructure.Abstractions.Contracts;

namespace TMS.Infrastructure.DataModels
{
    public class Department : IAuditableEntity
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
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }

        [Required]
        public Guid HeadId { get; set; }
        public User Head { get; set; }

        public string? Description { get; set; }

        [Required]
        public string ContactEmail { get; set; }
        public string? ContactPhone { get; set; }

        public List<Board> Boards { get; set; } = [];

        public Department() { }
    }
}
