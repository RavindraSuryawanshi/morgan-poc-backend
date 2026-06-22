using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.SalesforceSyncPOC.Core.Enums
{
    /// <summary>
    /// Represents the processing state of an outbox message.
    /// </summary>
    public enum OutboxStatus
    {
        Pending = 1,
        Processing = 2,
        Published = 3,
        DeadLetter = 4
    }
}
