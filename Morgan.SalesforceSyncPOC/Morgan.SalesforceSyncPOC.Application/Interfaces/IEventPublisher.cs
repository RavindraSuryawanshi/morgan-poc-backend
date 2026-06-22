

namespace Morgan.SalesforceSyncPOC.Application.Interfaces
{
    /// <summary>
    /// Publishes integration events to a messaging system.
    /// </summary>
    public interface IEventPublisher
    {
        Task PublishAsync(
            string subject,
            string payload,
            string correlationId,
            CancellationToken cancellationToken = default);
    }
}
