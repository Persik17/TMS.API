using System.ComponentModel.DataAnnotations;
using TMS.Infrastructure.Abstractions.Contracts;

namespace TMS.Infrastructure.DataModels
{
    public class TaskFile : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid TaskId { get; set; }
        public Task Task { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }

        [Required]
        public string ContentType { get; set; }

        [Required]
        public byte[] FileData { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
