using Morgan.Salesforce.POC.AzureFunctions.Models;
using Morgan.SalesforceSyncPOC.AzureFunctions.Models;

namespace Morgan.Salesforce.POC.AzureFunctions.Services
{
    /// <summary>
    /// Validates application data against Salesforce schema metadata.
    /// </summary>
    public interface ISchemaValidator
    {
        ValidationResult Validate(
            UserDto user,
            SalesforceObjectMetadata metadata);
    }
}
