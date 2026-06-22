using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Morgan.SalesforceSyncPOC.Application.Interfaces;

namespace Morgan.SalesforceSyncPOC.Integration.Outbox
{
    /// <summary>
    /// Background service that publishes pending outbox messages.
    /// </summary>
    public sealed class OutboxPublisherHostedService
    : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly OutboxPublisherOptions _options;
        private readonly ILogger<OutboxPublisherHostedService> _logger;

        public OutboxPublisherHostedService(
            IServiceScopeFactory serviceScopeFactory,
            IOptions<OutboxPublisherOptions> options,
            ILogger<OutboxPublisherHostedService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _options = options.Value;
            _logger = logger;
        }

        /// <summary>
        /// Continuously processes pending outbox messages.
        /// </summary>
        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox Publisher Hosted Service Started. Polling Interval: {PollingIntervalSeconds} seconds",
                _options.PollingIntervalSeconds);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var publisher = scope.ServiceProvider.GetRequiredService<IOutboxPublisher>();

                    await publisher.PublishPendingMessagesAsync(
                        stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Outbox Publisher Failed");
                }

                await Task.Delay(
                    TimeSpan.FromSeconds(
                        _options.PollingIntervalSeconds),
                    stoppingToken);
            }
        }
    }
}
