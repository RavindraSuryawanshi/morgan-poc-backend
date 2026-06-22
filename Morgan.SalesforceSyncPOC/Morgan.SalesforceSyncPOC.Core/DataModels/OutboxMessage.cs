using Morgan.SalesforceSyncPOC.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.SalesforceSyncPOC.Core.DataModels
{
    /// <summary>
    /// Represents an event stored in the outbox for reliable message publishing.
    /// </summary>
    public class OutboxMessage
    {
        public long Id { get; set; }

        /// <summary>
        /// Correlates related operations and messages across systems.
        /// </summary>
        public Guid CorrelationId { get; set; }

        public string EventType { get; set; } = "";

        public string Payload { get; set; } = "";

        public OutboxStatus Status { get; set; }

        /// <summary>
        /// Number of publish attempts made for the message.
        /// </summary>
        public int RetryCount { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? PublishedDate { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
