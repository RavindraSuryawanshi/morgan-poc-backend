using Morgan.SalesforceSyncPOC.AzureFunctions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.SalesforceSyncPOC.AzureFunctions.Services
{
    /// <summary>
    /// Provides authentication to Salesforce and returns access tokens.
    /// </summary>
    public interface ISalesforceAuthService
    {
        Task<SalesforceTokenResponse> AuthenticateAsync(
            CancellationToken cancellationToken);
    }
}
