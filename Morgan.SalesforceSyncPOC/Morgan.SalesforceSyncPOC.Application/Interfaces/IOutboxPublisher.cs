

namespace Morgan.SalesforceSyncPOC.Application.Interfaces
{
    /// <summary>
    /// Publishes pending messages from the outbox.
    /// </summary>
    public interface IOutboxPublisher
    {
        Task PublishPendingMessagesAsync(
            CancellationToken cancellationToken);
    }
}
