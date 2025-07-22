namespace TMS.API.ViewModels.Board
{
    public class BoardInfoViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OwnerFullName { get; set; }
        public bool IsPrivate { get; set; }
    }
}
