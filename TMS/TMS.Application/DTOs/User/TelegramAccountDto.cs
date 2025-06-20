namespace TMS.Application.DTOs.User
{
    public class TelegramAccountDto
    {
        public Guid Id { get; set; }
        public string NickName { get; set; }
        public string Phone { get; set; }
        public long ChatId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}