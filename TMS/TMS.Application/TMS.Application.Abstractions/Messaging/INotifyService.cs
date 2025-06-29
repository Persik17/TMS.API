using TMS.Contracts.Models;

namespace TMS.Application.Abstractions.Messaging
{
    /// <summary>
    /// Defines a contract for sending strongly-typed events to a message queue (e.g., RabbitMQ).
    /// </summary>
    public interface INotifyService
    {
        /// <summary>
        /// Publishes an event to the message queue for asynchronous processing.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event to publish. Must implement <see cref="IEvent"/>.</typeparam>
        /// <param name="event">The event instance to publish.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous publish operation.</returns>
        Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class, IEvent;
    }
}