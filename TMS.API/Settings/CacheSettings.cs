namespace TMS.API.Settings
{
    public class CacheSettings
    {
        public int CompanyTtlMinutes { get; set; }
        public int BoardTtlMinutes { get; set; }
        public int ColumnTtlMinutes { get; set; }
        public int UserTtlMinutes { get; set; }
    }
}
