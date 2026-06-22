using Microsoft.EntityFrameworkCore;
using Morgan.SalesforceSyncPOC.Application.Interfaces;
using Morgan.SalesforceSyncPOC.Core.DataModels;
using Morgan.SalesforceSyncPOC.Core.Enums;
using Morgan.SalesforceSyncPOC.Infrastrcture.Data;

namespace Morgan.SalesforceSyncPOC.Integration.Outbox
{
    /// <summary>
    /// Provides access to pending outbox messages.
    /// </summary>
    public sealed class OutboxRepository : IOutboxRepository
    {
        private readonly AppDbContext _dbContext;

        public OutboxRepository(
            AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves pending outbox messages for processing.
        /// </summary>
        public async Task<List<OutboxMessage>> GetPendingMessagesAsync(
            int batchSize,
            int maxRetryCount,
            CancellationToken cancellationToken)
        {
            return await _dbContext.OutboxMessages
                .Where(x =>
                    x.Status == OutboxStatus.Pending &&
                    x.RetryCount < maxRetryCount)
                .OrderBy(x => x.Id)
                .Take(batchSize)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Updates an outbox message.
        /// </summary>
        public async Task UpdateAsync(
            OutboxMessage message,
            CancellationToken cancellationToken)
        {
            _dbContext.OutboxMessages.Update(message);

            await _dbContext.SaveChangesAsync(
                cancellationToken);
        }
    }

}
