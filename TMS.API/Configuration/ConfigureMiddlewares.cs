using TMS.API.Middleware;

namespace TMS.API.Configuration
{
    /// <summary>
    /// Provides extension methods for configuring application middleware.
    /// </summary>
    public static class ConfigureMiddlewares
    {
        /// <summary>
        /// Applies the specified middleware to the application pipeline.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance, allowing for chaining.</returns>
        public static IApplicationBuilder Apply(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
