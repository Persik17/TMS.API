namespace TMS.Application.Dto.Task
{
    public class TaskCreateDto
    {
        public string Name { get; set; }
        public Guid BoardId { get; set; }
        public Guid CreatorId { get; set; }
        public Guid AssigneeId { get; set; }
        public Guid TaskTypeId { get; set; }
        public Guid ColumnId { get; set; }
    }
}