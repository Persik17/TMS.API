using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TMS.Infrastructure.DataAccess.Contexts
{
    public class PostgreSqlTmsContextFactory : IDesignTimeDbContextFactory<PostgreSqlTmsContext>
    {
        public PostgreSqlTmsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlTmsContext>();

            var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
            optionsBuilder.UseNpgsql(connectionString);

            return new PostgreSqlTmsContext(optionsBuilder.Options);
        }
    }
}