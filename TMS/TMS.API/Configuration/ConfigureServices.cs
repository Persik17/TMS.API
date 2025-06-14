using Microsoft.EntityFrameworkCore;
using TMS.Application;
using TMS.Infrastructure.DataAccess;
using TMS.Infrastructure.DataAccess.Contexts;

namespace TMS.API.Configuration
{
    public static class ConfigureServices
    {
        /// <summary>
        /// Configures application and infrastructure services.
        /// </summary>
        /// <param name="builder">The WebApplicationBuilder instance.</param>
        public static void Apply(WebApplicationBuilder builder)
        {
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureRepositories();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<PostgreSqlTmsContext>(options =>
                options.UseNpgsql("Host=localhost;Port=5432;Database=otusTMS;Username=postgres;Password=3353"));
        }
    }
}
