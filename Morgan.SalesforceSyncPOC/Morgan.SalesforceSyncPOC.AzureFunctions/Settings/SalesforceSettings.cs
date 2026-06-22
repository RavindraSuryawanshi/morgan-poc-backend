using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.Salesforce.POC.AzureFunctions.Settings
{
    /// <summary>
    /// Configuration settings for Salesforce integration.
    /// </summary>
    public sealed class SalesforceSettings
    {
        public string InstanceUrl { get; set; } = string.Empty;

        public string ClientId { get; set; } = string.Empty;

        public string ClientSecret { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string SecurityToken { get; set; } = string.Empty;

        public string ApiVersion { get; set; } = "v61.0";

        public string ObjectName { get; set; } = "Customer__c";
    }
}
