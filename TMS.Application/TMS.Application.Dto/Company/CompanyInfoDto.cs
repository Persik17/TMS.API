using TMS.Application.Dto.Board;

namespace TMS.Application.Dto.Company
{
    public class CompanyInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string INN { get; set; }
        public string OGRN { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string Industry { get; set; }
        public string Description { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public bool IsActive { get; set; }
        public List<BoardInfoDto> Boards { get; set; } = [];
    }
}
