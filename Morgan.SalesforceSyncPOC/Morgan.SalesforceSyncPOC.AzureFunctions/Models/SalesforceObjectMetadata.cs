using Morgan.Salesforce.POC.AzureFunctions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.SalesforceSyncPOC.AzureFunctions.Models
{
    /// <summary>
    /// Salesforce object metadata returned by the describe API.
    /// </summary>
    public sealed class SalesforceObjectMetadata
    {
        public List<SalesforceFieldInfo> fields { get; set; }
            = new();
    }
}
