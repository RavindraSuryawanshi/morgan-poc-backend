using Morgan.SalesforceSyncPOC.AzureFunctions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.Salesforce.POC.AzureFunctions.Services
{
    /// <summary>
    /// Provides Salesforce object metadata retrieval operations.
    /// </summary>
    public interface ISalesforceMetadataService
    {
        Task<SalesforceObjectMetadata>
            GetObjectMetadataAsync(
                CancellationToken cancellationToken);
    }
}
