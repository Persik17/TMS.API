namespace TMS.Application.Dto.Column
{
    public class ColumnDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ColumnType { get; set; }
        public int Order { get; set; }
        public string Color { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}