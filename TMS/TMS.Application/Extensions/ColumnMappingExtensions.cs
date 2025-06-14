using TMS.Application.Models.DTOs.Column;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Extensions
{
    public static class ColumnMappingExtensions
    {
        public static Column ToColumn(this ColumnCreateDto dto)
        {
            return new Column
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                ColumnType = dto.ColumnType,
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
                ColumnType = column.ColumnType,
                Order = column.Order,
                Color = column.Color,
                CreationDate = column.CreationDate,
                UpdateDate = column.UpdateDate,
                DeleteDate = column.DeleteDate
            };
        }
    }
}