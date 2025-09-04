namespace TMS.Application.Dto.Task
{
    public class TaskUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid? AssigneeId { get; set; }
        public int? StoryPoints { get; set; }
        public int? Priority { get; set; }
        public int? Severity { get; set; }
    }
}
