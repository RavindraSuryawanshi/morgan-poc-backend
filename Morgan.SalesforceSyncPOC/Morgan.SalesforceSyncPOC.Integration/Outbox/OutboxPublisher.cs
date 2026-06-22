using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Morgan.SalesforceSyncPOC.Application.Interfaces;
using Morgan.SalesforceSyncPOC.Core.Enums;
using Morgan.SalesforceSyncPOC.Infrastrcture.Data;
using Morgan.SalesforceSyncPOC.Integration.ServiceBus;
using Serilog.Context;

namespace Morgan.SalesforceSyncPOC.Integration.Outbox
{
    /// <summary>
    /// Publishes pending outbox messages to external systems.
    /// </summary>
    public sealed class OutboxPublisher : IOutboxPublisher
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly OutboxPublisherOptions _options;
        private readonly ILogger<OutboxPublisher> _logger;
        private readonly IOutboxRepository _outboxRepository;
        public OutboxPublisher(
            IOutboxRepository outboxRepository,
            IOptions<OutboxPublisherOptions> options,
            ILogger<OutboxPublisher> logger,
            IEventPublisher eventPublisher)
        {
            _outboxRepository = outboxRepository;
            _options = options.Value;
            _logger = logger;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Processes and publishes pending outbox messages.
        /// </summary>
        public async Task PublishPendingMessagesAsync(
            CancellationToken cancellationToken)
        {


            var messages = await _outboxRepository.GetPendingMessagesAsync(
                        _options.BatchSize,
                        _options.MaxRetryCount,
                        cancellationToken);            

            if (!messages.Any())
            {
                return;
            }

            _logger.LogInformation("Found {MessageCount} pending outbox messages for publishing",
                messages.Count);

            foreach (var message in messages)
            {
                using (LogContext.PushProperty("ApplicationName", "BackgroundWorker"))
                using (LogContext.PushProperty("CorrelationId", message.CorrelationId))
                using (LogContext.PushProperty("EventId", message.Id))
                using (LogContext.PushProperty("EventType", message.EventType))
                {
                    try
                    {
                        _logger.LogInformation("Publishing outbox message");
                        message.Status = OutboxStatus.Processing;
                        await _outboxRepository.UpdateAsync(message, cancellationToken);

                        await _eventPublisher.PublishAsync(
                            message.EventType,
                            message.Payload,
                            message.CorrelationId.ToString(),
                            cancellationToken);

                        message.Status = OutboxStatus.Published;
                        message.PublishedDate = DateTime.UtcNow;
                        message.ErrorMessage = null;
                        await _outboxRepository.UpdateAsync(message, cancellationToken);

                        _logger.LogInformation("Outbox message published successfully");
                    }
                    catch (Exception ex)
                    {
                        message.RetryCount++;
                        message.ErrorMessage = ex.Message;

                        if (message.RetryCount >= _options.MaxRetryCount)
                        {
                            message.Status = OutboxStatus.DeadLetter;
                        }
                        else
                        {
                            message.Status = OutboxStatus.Pending;
                        }
                        await _outboxRepository.UpdateAsync(message, cancellationToken);

                        _logger.LogError(ex, "Failed to publish outbox message. RetryCount: {RetryCount}",
                        message.RetryCount);
                    }
                }
            }
        }
    }
}