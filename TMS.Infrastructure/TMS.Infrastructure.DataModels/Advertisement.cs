using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.Infrastructure.DataModels;

[Table("Advertisement")]
public class Advertisement
{
    [Key] [Column("id")] public Guid Id { get; set; }

    [Required]
    [Column("title")]
    [MaxLength(200)]
    public string Title { get; set; }

    [Required] [Column("content")] public string Content { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; set; }

    [Required] [Column("user_id")] public Guid UserId { get; set; }

    [ForeignKey("UserId")] public virtual User User { get; set; }
}