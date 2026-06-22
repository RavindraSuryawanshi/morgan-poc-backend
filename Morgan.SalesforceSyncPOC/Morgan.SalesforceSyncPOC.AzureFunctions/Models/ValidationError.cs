using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.SalesforceSyncPOC.AzureFunctions.Models
{
    /// <summary>
    /// Represents a validation error for a specific field.
    /// </summary>
    public sealed class ValidationError
    {
        public string FieldName { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}
