using Morgan.SalesforceSyncPOC.Core.DataModels;


namespace Morgan.SalesforceSyncPOC.Application.Interfaces
{
    /// <summary>
    /// Provides access to outbox messages used for event publishing.
    /// </summary>
    public interface IOutboxRepository
    {
        Task<List<OutboxMessage>> GetPendingMessagesAsync(
            int batchSize,
            int maxRetryCount,
            CancellationToken cancellationToken);

        Task UpdateAsync(
            OutboxMessage message,
            CancellationToken cancellationToken);
    }
}
