namespace TMS.Application.Dto.Task
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid BoardId { get; set; }
        public string BoardName { get; set; }
        public Guid CreatorId { get; set; }
        public string CreatorName { get; set; }
        public Guid? AssigneeId { get; set; }
        public string AssigneeName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ActualClosingDate { get; set; }
        public int? StoryPoints { get; set; }
        public Guid TaskTypeId { get; set; }
        public int? Priority { get; set; }
        public int? Severity { get; set; }
        public Guid? ParentTaskId { get; set; }
        public Guid ColumnId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}