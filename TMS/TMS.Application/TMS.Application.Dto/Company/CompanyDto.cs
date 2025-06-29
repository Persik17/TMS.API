namespace TMS.Application.Dto.Company
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string INN { get; set; }
        public string OGRN { get; set; }
        public string Address { get; set; }
        public string Logo { get; set; }
        public string Website { get; set; }
        public string Industry { get; set; }
        public string Description { get; set; }
        public Guid OwnerId { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        // public UserDto Owner { get; set; } // если потребуется, добавить потом
    }
}
