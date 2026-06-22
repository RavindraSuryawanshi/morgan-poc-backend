using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.SalesforceSyncPOC.Core.Events
{
    /// <summary>
    /// User-related event names used for messaging and integration.
    /// </summary>
    public static class UserEvents
    {
        public const string Created = "UserCreated";

        public const string Updated = "UserUpdated";

        public const string Deleted = "UserDeleted";
    }
}
