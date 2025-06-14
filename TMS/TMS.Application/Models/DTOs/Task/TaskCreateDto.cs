namespace TMS.Application.Models.DTOs.Task
{
    public class TaskCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid BoardId { get; set; }
        public Guid CreatorId { get; set; }
        public Guid AssigneeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StoryPoints { get; set; }
        public Guid TaskTypeId { get; set; }
        public int Priority { get; set; }
        public int Severity { get; set; }
        public Guid? ParentTaskId { get; set; }
        public Guid ColumnId { get; set; }
    }
}