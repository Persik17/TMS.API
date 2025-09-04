using System.ComponentModel.DataAnnotations;
using TMS.Infrastructure.Abstractions.Contracts;

namespace TMS.Infrastructure.DataModels
{
    public class SystemSettings : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        public int Theme { get; set; }

        [MaxLength(255)]
        public string? BoardBackgroundUrl { get; set; }

        [MaxLength(100)]
        public string? BoardBackgroundName { get; set; }
    }
}
