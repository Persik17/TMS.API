namespace TMS.Application.Dto.Comment
{
    /// <summary>
    /// Represents the data transfer object (DTO) for a comment.
    /// </summary>
    public class CommentDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the comment.
        /// </summary>
        public Guid Id { get; set; }

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

        /// <summary>
        /// Gets or sets the date and time when the comment was created.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the comment was last updated. This property is nullable.
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the comment was deleted. This property is nullable.
        /// </summary>
        public DateTime? DeleteDate { get; set; }
    }
}