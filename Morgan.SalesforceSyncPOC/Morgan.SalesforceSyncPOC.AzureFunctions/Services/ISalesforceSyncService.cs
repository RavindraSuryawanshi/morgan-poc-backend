using Morgan.SalesforceSyncPOC.AzureFunctions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.Salesforce.POC.AzureFunctions.Services
{
    /// <summary>
    /// Handles synchronization of user data with Salesforce.
    /// </summary>
    public interface ISalesforceSyncService
    {
        Task UpsertCustomerAsync(
            UserDto user,
            CancellationToken cancellationToken);
    }
}
