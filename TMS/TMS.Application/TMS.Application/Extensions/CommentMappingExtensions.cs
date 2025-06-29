using TMS.Application.Dto.Comment;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Extensions
{
    public static class CommentMappingExtensions
    {
        public static Comment ToComment(this CommentCreateDto dto)
        {
            return new Comment
            {
                Id = Guid.NewGuid(),
                Text = dto.Text,
                TaskId = dto.TaskId,
                UserId = dto.AuthorId,
                CreationDate = DateTime.UtcNow
            };
        }

        public static CommentDto ToCommentDto(this Comment entity)
        {
            return new CommentDto
            {
                Id = entity.Id,
                Text = entity.Text,
                TaskId = entity.TaskId,
                AuthorId = entity.UserId,
                CreationDate = entity.CreationDate,
                UpdateDate = entity.UpdateDate,
                DeleteDate = entity.DeleteDate
            };
        }
    }
}