using MassTransit;

namespace TMS.API.Configuration
{
    /// <summary>
    /// Provides methods for configuring RabbitMQ as a message broker using MassTransit.
    /// </summary>
    public static class ConfigureRabbitMQ
    {
        /// <summary>
        /// Configures RabbitMQ for use with MassTransit.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add MassTransit and RabbitMQ to.</param>
        public static void Apply(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });
            });
        }
    }
}
