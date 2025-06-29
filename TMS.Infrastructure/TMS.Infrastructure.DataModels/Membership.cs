using TMS.Infrastructure.Abstractions.Contracts;

namespace TMS.Infrastructure.DataModels
{
    public class Membership : IAuditableEntity
    {
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ResourceId { get; set; } // FK to Company/Department/Board (not enforced)
        public int ResourceType { get; set; }

        public Guid RoleId { get; set; } 
        public Role Role { get; set; }
    }
}
