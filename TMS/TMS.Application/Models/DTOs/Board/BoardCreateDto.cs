namespace TMS.Application.DTOs.Board
{
    public class BoardCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid DepartmentId { get; set; }
        public int BoardType { get; set; }
        public bool IsPrivate { get; set; }
    }
}