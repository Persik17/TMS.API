using TMS.Application.Abstractions.Factories;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Factories
{
    public class ColumnFactory : IColumnFactory
    {
        public IEnumerable<Column> CreateDefaultColumns(Guid boardId)
        {
            var now = DateTime.UtcNow;
            return
            [
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Бэклог",
                    Description = "Бэклог",
                    Order = 0,
                    Color = "#B0BEC5",
                    BoardId = boardId,
                    CreationDate = now
                },
                new() {
                    Id = Guid.NewGuid(),
                    Name = "В работе",
                    Description = "В работе",
                    Order = 1,
                    Color = "#42A5F5",
                    BoardId = boardId,
                    CreationDate = now
                },
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Закрыто",
                    Description = "Закрыто",
                    Order = 2,
                    Color = "#66BB6A",
                    BoardId = boardId,
                    CreationDate = now
                }
            ];
        }
    }
}
