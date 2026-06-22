using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.SalesforceSyncPOC.Integration.Outbox
{
    /// <summary>
    /// Outbox publisher configuration settings.
    /// </summary>
    public sealed class OutboxPublisherOptions
    {
        public int BatchSize { get; set; } = 100;

        public int PollingIntervalSeconds { get; set; } = 10;

        public int MaxRetryCount { get; set; } = 5;
    }
}
