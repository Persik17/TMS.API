using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using TMS.Application;
using TMS.Infrastructure.Abstractions.Cache;
using TMS.Infrastructure.DataAccess;
using TMS.Infrastructure.DataAccess.Contexts;

namespace TMS.API.Configuration
{
    /// <summary>
    /// Provides methods for configuring application services, middleware, and infrastructure components.
    /// </summary>
    public static class ConfigureServices
    {
        /// <summary>
        /// Configures application and infrastructure services, including data access, caching, and API features.
        /// </summary>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance used to configure the application.</param>
        public static void Apply(WebApplicationBuilder builder)
        {
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureRepositories();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<PostgreSqlTmsContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("POSTGRES_CONNECTION_STRING")));

            var redisConnection = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));

            builder.Services.AddTransient<IRedisCacheContext, RedisCacheContext>();
        }
    }
}
