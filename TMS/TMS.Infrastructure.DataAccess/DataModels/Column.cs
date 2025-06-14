using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TMS.Abstractions.Models.Interfaces;

namespace TMS.Infrastructure.DataAccess.DataModels
{
    public class Column : IAuditableEntity
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

        public string Description { get; set; }

        public int ColumnType { get; set; }

        public int Order { get; set; }

        [MaxLength(7)] //#RRGGBB
        public string Color { get; set; }

        public Column() { }
    }
}
