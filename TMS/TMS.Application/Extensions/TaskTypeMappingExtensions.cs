using TMS.Application.DTOs.TaskType;
using TMS.Infrastructure.DataAccess.DataModels;

namespace TMS.Application.Extensions
{
    public static class TaskTypeMappingExtensions
    {
        public static TaskType ToTaskType(this TaskTypeCreateDto dto)
        {
            return new TaskType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                CreationDate = DateTime.UtcNow
            };
        }

        public static TaskTypeDto ToTaskTypeDto(this TaskType entity)
        {
            return new TaskTypeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CreationDate = entity.CreationDate,
                UpdateDate = entity.UpdateDate,
                DeleteDate = entity.DeleteDate
            };
        }
    }
}