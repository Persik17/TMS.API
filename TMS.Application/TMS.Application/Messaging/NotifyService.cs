using MassTransit;
using TMS.Application.Abstractions.Messaging;
using TMS.Contracts.Models;

namespace TMS.Application.Messaging
{
    /// <summary>
    /// Provides a service for publishing events to a message bus.
    /// </summary>
    public class NotifyService : INotifyService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyService"/> class.
        /// </summary>
        /// <param name="publishEndpoint">The MassTransit publish endpoint for publishing events.</param>
        public NotifyService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        /// <summary>
        /// Publishes an event to the message bus.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event to publish. Must be a class that implements <see cref="IEvent"/>.</typeparam>
        /// <param name="event">The event to publish.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the publish operation.</param>
        /// <returns>A task that represents the asynchronous publish operation.</returns>
        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class, IEvent
        {
            await _publishEndpoint.Publish(@event, cancellationToken);
        }
    }
}