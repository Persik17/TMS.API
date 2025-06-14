using MassTransit;
using TMS.Abstractions.Interfaces.Services;
using TMS.Contracts.Models;

namespace TMS.Application.Services
{
    public class NotifyService : INotifyService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public NotifyService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class, IEvent
        {
            await _publishEndpoint.Publish(@event, cancellationToken);
        }
    }
}