using MassTransit;

namespace TMS.API.Configuration
{
    public static class ConfigureRabbitMQ
    {
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
