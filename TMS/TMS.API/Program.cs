using TMS.API.Configuration;

namespace TMS.API
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method which configures and runs the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure logging
            ConfigureLogging.Apply(builder.Logging);

            // Configure services
            ConfigureServices.Apply(builder);

            // Add controllers, API explorer, and Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure RabbitMQ
            ConfigureRabbitMQ.Apply(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Apply global exception handling
            ConfigureMiddlewares.Apply(app);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}