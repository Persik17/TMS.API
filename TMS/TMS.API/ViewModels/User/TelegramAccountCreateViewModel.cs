namespace TMS.API.ViewModels.User
{
    public class TelegramAccountCreateViewModel
    {
        public long Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Username { get; set; }
        public string Photo_Url { get; set; }
        public int Auth_Date { get; set; }
        public string Hash { get; set; }
    }
}