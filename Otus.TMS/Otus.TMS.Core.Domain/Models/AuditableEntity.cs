namespace Otus.TMS.Domain.Models
{
    public class AuditableEntity : IAuditableEntity
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
