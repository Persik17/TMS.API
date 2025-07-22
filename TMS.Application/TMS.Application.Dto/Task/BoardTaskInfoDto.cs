namespace TMS.Application.Dto.Task
{
    public class BoardTaskInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? AssigneeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? StoryPoints { get; set; }
        public int? Priority { get; set; }
        public Guid TaskTypeId { get; set; }
    }
}
