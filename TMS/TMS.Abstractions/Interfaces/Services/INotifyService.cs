using TMS.Contracts.Models;

namespace TMS.Abstractions.Interfaces.Services
{
    /// <summary>
    /// Defines a contract for sending strongly-typed events to a message queue (e.g., RabbitMQ).
    /// </summary>
    public interface INotifyService
    {
        /// <summary>
        /// Publishes an event to the message queue for asynchronous processing by other services or components.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event to publish. Must implement <see cref="IEvent"/>.</typeparam>
        /// <param name="event">The event instance to publish.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous publish operation.</returns>
        Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class, IEvent;
    }
}