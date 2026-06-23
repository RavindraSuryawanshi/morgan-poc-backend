using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.SalesforceSyncPOC.Core.Events
{
    /// <summary>
    /// Event names used for messaging and integration.
    /// </summary>
    public static class EventNames
    {
        public const string Created = "Created";

        public const string Updated = "Updated";

        public const string Deleted = "Deleted";
    }
}
