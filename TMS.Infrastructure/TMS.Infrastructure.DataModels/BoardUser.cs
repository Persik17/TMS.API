namespace TMS.Infrastructure.DataModels
{
    public class BoardUser
    {
        public Guid BoardsId { get; set; }
        public Board Board { get; set; }

        public Guid UsersId { get; set; }
        public User User { get; set; }
    }
}
