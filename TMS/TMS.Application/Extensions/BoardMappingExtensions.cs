using TMS.Application.DTOs.Board;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Extensions
{
    public static class BoardMappingExtensions
    {
        public static Board ToBoard(this BoardCreateDto dto)
        {
            return new Board
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                DepartmentId = dto.DepartmentId,
                BoardType = dto.BoardType,
                IsPrivate = dto.IsPrivate,
                CreationDate = DateTime.UtcNow
            };
        }

        public static BoardDto ToBoardDto(this Board board)
        {
            return new BoardDto
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                DepartmentId = board.DepartmentId,
                BoardType = board.BoardType,
                IsPrivate = board.IsPrivate,
                CreationDate = board.CreationDate,
                UpdateDate = board.UpdateDate,
                DeleteDate = board.DeleteDate
            };
        }
    }
}