namespace TMS.Application.Dto.Task
{
    public class TaskFileDto
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] FileData { get; set; }
    }
}
