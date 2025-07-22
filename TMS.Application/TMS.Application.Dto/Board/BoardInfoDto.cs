namespace TMS.Application.Dto.Board
{
    public class BoardInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OwnerFullName { get; set; }
        public bool IsPrivate { get; set; }
    }
}
