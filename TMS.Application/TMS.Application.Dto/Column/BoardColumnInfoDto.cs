using TMS.Application.Dto.Task;

namespace TMS.Application.Dto.Column
{
    public class BoardColumnInfoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public int Order { get; set; }
        public List<BoardTaskInfoDto> Tasks { get; set; } = [];
    }
}
