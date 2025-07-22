using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TMS.Infrastructure.DataAccess.Contexts
{
    public class PostgreSqlTmsContextFactory : IDesignTimeDbContextFactory<PostgreSqlTmsContext>
    {
        public PostgreSqlTmsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlTmsContext>();

            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TMS;Username=postgres;Password=3353");

            return new PostgreSqlTmsContext(optionsBuilder.Options);
        }
    }
}