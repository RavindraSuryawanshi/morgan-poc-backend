using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.Salesforce.POC.AzureFunctions.Models
{
    /// <summary>
    /// Event published when a user is created.
    /// </summary>
    public sealed class UserCreatedEvent
    {
        public Guid EventId { get; set; }

        public Guid CorrelationId { get; set; }

        public int UserId { get; set; }

        public DateTime OccurredOn { get; set; }
    }
}
