using TMS.Application.Dto.Column;
using TMS.Application.Dto.TaskType;

namespace TMS.Application.Dto.Board
{
    public class BoardSummaryInfoDto
    {
        public Guid BoardId { get; set; }
        public List<BoardColumnInfoDto> Columns { get; set; } = [];
        public List<BoardTaskTypeDto> TaskTypes { get; set; } = [];
    }
}
