using System.ComponentModel.DataAnnotations;
using TMS.Infrastructure.Abstractions.Contracts;

namespace TMS.Infrastructure.DataModels
{
    public class Task : IAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public Guid BoardId { get; set; }
        public Board Board { get; set; }

        [Required]
        public Guid CreatorId { get; set; }
        public User Creator { get; set; }

        public Guid? AssigneeId { get; set; }
        public User? Assignee { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ActualClosingDate { get; set; }

        public int? StoryPoints { get; set; }

        [Required]
        public Guid TaskTypeId { get; set; }
        public TaskType TaskType { get; set; }

        public int? Priority { get; set; }
        public int? Severity { get; set; }

        public Guid? ParentTaskId { get; set; }
        public Task? ParentTask { get; set; }

        [Required]
        public Guid ColumnId { get; set; }
        public Column Column { get; set; }

        public Task() { }
    }
}
