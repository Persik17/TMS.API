namespace TMS.Abstractions.Models.DTOs
{
    public class CommentCreateDto
    {
        public string Text { get; set; }
        public Guid TaskId { get; set; }
        public Guid AuthorId { get; set; }
    }
}