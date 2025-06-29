namespace TMS.Application.Dto.Comment
{
    /// <summary>
    /// Represents the data transfer object (DTO) for creating a new comment.
    /// </summary>
    public class CommentCreateDto
    {
        /// <summary>
        /// Gets or sets the text content of the comment.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the task to which the comment belongs.
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the author who created the comment.
        /// </summary>
        public Guid AuthorId { get; set; }
    }
}