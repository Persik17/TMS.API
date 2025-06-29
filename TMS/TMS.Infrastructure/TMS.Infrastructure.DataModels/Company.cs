using System.ComponentModel.DataAnnotations;
using TMS.Infrastructure.Abstractions.Contracts;

namespace TMS.Infrastructure.DataModels
{
    public class Company : IAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string INN { get; set; }
        public string OGRN { get; set; }
        public string Address { get; set; }
        public string Logo { get; set; }
        public string Website { get; set; }
        public string Industry { get; set; }
        public string Description { get; set; }

        [Required]
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }

        [Required]
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        public List<Department> Departments { get; set; } = [];

        public Company() { }
    }
}
