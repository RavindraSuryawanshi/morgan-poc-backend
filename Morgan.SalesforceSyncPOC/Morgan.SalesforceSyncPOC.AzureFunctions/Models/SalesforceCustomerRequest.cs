

namespace Morgan.SalesforceSyncPOC.AzureFunctions.Models
{
    /// <summary>
    /// Salesforce customer payload used for create and update operations.
    /// </summary>
    public sealed class SalesforceCustomerRequest
    {

        public string FirstName__c { get; set; } = string.Empty;

        public string LastName__c { get; set; } = string.Empty;

        public string Email__c { get; set; } = string.Empty;

        public string? Phone__c { get; set; }
    }
}
