using Otus.TMS.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Otus.TMS.Infrastructure.DataAccess.DataModels
{
    public class Company : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string INN { get; set; }
        public string OGRN { get; set; }
        public string Address { get; set; }
        public string Logo { get; set; }
        public string Website { get; set; }
        public string Industry { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        public List<Department> Departments { get; set; } = [];

        public Company() { }
    }
}
