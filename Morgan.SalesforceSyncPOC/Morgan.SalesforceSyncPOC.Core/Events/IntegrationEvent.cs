

namespace Morgan.SalesforceSyncPOC.Core.Events
{
    public sealed class IntegrationEvent
    {
        public Guid EventId { get; set; }

        public Guid CorrelationId { get; set; }

        public string EntityName { get; set; } = string.Empty;

        public int EntityId { get; set; }

        public string EventType { get; set; } = string.Empty;

        public DateTime OccurredOn { get; set; }
    }
}
