using Morgan.SalesforceSyncPOC.AzureFunctions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.SalesforceSyncPOC.AzureFunctions.Exceptions
{
    /// <summary>
    /// Exception thrown when Salesforce schema validation fails.
    /// </summary>
    public sealed class SchemaValidationException
    : Exception
    {
        /// <summary>
        /// Validation errors that caused the failure.
        /// </summary>
        public IReadOnlyCollection<ValidationError> Errors
        {
            get;
        }

        public SchemaValidationException(
            IReadOnlyCollection<ValidationError> errors)
            : base("Salesforce schema validation failed.")
        {
            Errors = errors;
        }
    }
}
