namespace TMS.Application.Dto.Task
{
    public class CreatedTaskDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid BoardId { get; set; }
        public int? Priority { get; set; }
        public int? StoryPoints { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid CreatorId { get; set; }
        public Guid? AssigneeId { get; set; }
        public Guid TaskTypeId { get; set; }
        public Guid ColumnId { get; set; }
    }
}