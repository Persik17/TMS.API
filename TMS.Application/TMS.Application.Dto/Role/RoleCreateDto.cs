namespace TMS.Application.Dto.Role
{
    public class RoleCreateDto
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}