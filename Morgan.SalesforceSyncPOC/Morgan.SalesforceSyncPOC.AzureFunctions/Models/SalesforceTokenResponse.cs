

namespace Morgan.SalesforceSyncPOC.AzureFunctions.Models
{
    /// <summary>
    /// Salesforce OAuth token response.
    /// </summary>
    public sealed class SalesforceTokenResponse
    {
        public string access_token { get; set; } = string.Empty;

        public string instance_url { get; set; } = string.Empty;

        public string token_type { get; set; } = string.Empty;
    }
}
