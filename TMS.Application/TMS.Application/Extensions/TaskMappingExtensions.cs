using TMS.Application.Dto.Task;

namespace TMS.Application.Extensions
{
    public static class TaskMappingExtensions
    {
        public static Infrastructure.DataModels.Task ToTask(this TaskCreateDto dto)
        {
            return new Infrastructure.DataModels.Task
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                BoardId = dto.BoardId,
                CreatorId = dto.CreatorId,
                AssigneeId = dto.AssigneeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                StoryPoints = dto.StoryPoints,
                TaskTypeId = dto.TaskTypeId,
                Priority = dto.Priority,
                Severity = dto.Severity,
                ParentTaskId = dto.ParentTaskId,
                ColumnId = dto.ColumnId,
                CreationDate = DateTime.UtcNow
            };
        }

        public static TaskDto ToTaskDto(this Infrastructure.DataModels.Task entity)
        {
            return new TaskDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                BoardId = entity.BoardId,
                CreatorId = entity.CreatorId,
                AssigneeId = entity.AssigneeId,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                ActualClosingDate = entity.ActualClosingDate,
                StoryPoints = entity.StoryPoints,
                TaskTypeId = entity.TaskTypeId,
                Priority = entity.Priority,
                Severity = entity.Severity,
                ParentTaskId = entity.ParentTaskId,
                ColumnId = entity.ColumnId,
                CreationDate = entity.CreationDate,
                UpdateDate = entity.UpdateDate,
                DeleteDate = entity.DeleteDate
            };
        }
    }
}