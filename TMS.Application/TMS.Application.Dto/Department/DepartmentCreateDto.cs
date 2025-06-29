namespace TMS.Application.Dto.Department
{
    public class DepartmentCreateDto
    {
        public string Name { get; set; }
        public Guid CompanyId { get; set; }
        public Guid HeadId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
    }
}
