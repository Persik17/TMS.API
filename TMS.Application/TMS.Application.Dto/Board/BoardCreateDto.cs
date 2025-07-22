namespace TMS.Application.Dto.Board
{
    public class BoardCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public int BoardType { get; set; }
        public bool IsPrivate { get; set; }
    }
}