using TMS.Infrastructure.DataModels;

namespace TMS.Application.Abstractions.Factories
{
    public interface IColumnFactory
    {
        IEnumerable<Column> CreateDefaultColumns(Guid boardId);
    }
}
