

namespace Morgan.Salesforce.POC.AzureFunctions.Models
{
    /// <summary>
    /// Salesforce field metadata information.
    /// </summary>
    public sealed class SalesforceFieldInfo
    {
        public string name { get; set; } = string.Empty;

        public string type { get; set; } = string.Empty;

        public int length { get; set; }
    }
}
