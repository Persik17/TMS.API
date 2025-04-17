namespace Otus.TMS.Domain.Models
{
    public interface IAuditableEntity : IEntity
    {
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
