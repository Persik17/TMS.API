using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TMS.Abstractions.Models.Contracts;

namespace TMS.Infrastructure.DataAccess.DataModels
{
    public class BoardUserRole : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid BoardId { get; set; }
        public Board Board { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public BoardUserRole() { }
    }
}
