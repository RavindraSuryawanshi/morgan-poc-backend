using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Morgan.SalesforceSyncPOC.Application.Interfaces;


namespace Morgan.SalesforceSyncPOC.Integration.ServiceBus
{
    /// <summary>
    /// Publishes integration events to Azure Service Bus.
    /// </summary>
    public sealed class ServiceBusPublisher : IEventPublisher
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusSettings _settings;
        private readonly ILogger<ServiceBusPublisher> _logger;

        public ServiceBusPublisher(
            ServiceBusClient serviceBusClient,
            IOptions<ServiceBusSettings> settings,
            ILogger<ServiceBusPublisher> logger)
        {
            _serviceBusClient = serviceBusClient;
            _settings = settings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Publishes an event message to the configured topic.
        /// </summary>
        public async Task PublishAsync(
            string subject,
            string payload,
            string correlationId,
            CancellationToken cancellationToken = default)
        {
            var sender =
                _serviceBusClient.CreateSender(
                    _settings.TopicName);

            var message =
                new ServiceBusMessage(payload)
                {
                    Subject = subject,
                    CorrelationId = correlationId
                };

            await sender.SendMessageAsync(
                message,
                cancellationToken);

            _logger.LogInformation(
                "Published message to topic {Topic}",
                _settings.TopicName);
        }
    }
}
