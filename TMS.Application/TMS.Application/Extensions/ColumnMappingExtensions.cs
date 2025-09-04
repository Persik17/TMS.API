using TMS.Application.Dto.Column;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Extensions
{
    public static class ColumnMappingExtensions
    {
        public static Column ToColumn(this ColumnCreateDto dto)
        {
            return new Column
            {
                Id = Guid.NewGuid(),
                BoardId = dto.BoardId,
                Name = dto.Name,
                Description = dto.Description,
                Order = dto.Order,
                Color = dto.Color,
                CreationDate = DateTime.UtcNow
            };
        }

        public static ColumnDto ToColumnDto(this Column column)
        {
            return new ColumnDto
            {
                Id = column.Id,
                Name = column.Name,
                Description = column.Description,
                Order = column.Order,
                Color = column.Color,
                CreationDate = column.CreationDate,
                UpdateDate = column.UpdateDate,
                DeleteDate = column.DeleteDate
            };
        }
    }
}