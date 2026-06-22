
namespace Morgan.SalesforceSyncPOC.Integration.ServiceBus
{
    /// <summary>
    /// Service Bus configuration settings.
    /// </summary>
    public sealed class ServiceBusSettings
    {
        public string ConnectionString { get; set; } = string.Empty;

        public string TopicName { get; set; } = string.Empty;
    }
}
