using Morgan.SalesforceSyncPOC.AzureFunctions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.Salesforce.POC.AzureFunctions.Models
{
    /// <summary>
    /// Represents the result of a validation operation.
    /// </summary>
    public sealed class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;

        public List<ValidationError> Errors { get; set; }
            = new();
    }
}
