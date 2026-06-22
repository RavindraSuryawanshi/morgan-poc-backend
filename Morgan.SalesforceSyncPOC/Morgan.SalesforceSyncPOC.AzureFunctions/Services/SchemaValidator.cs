using global::Morgan.SalesforceSyncPOC.AzureFunctions.Models;
using Morgan.Salesforce.POC.AzureFunctions.Models;

namespace Morgan.Salesforce.POC.AzureFunctions.Services
{
    /// <summary>
    /// Validates application data against Salesforce schema metadata.
    /// </summary>
    public sealed class SchemaValidator : ISchemaValidator
    {
        public ValidationResult Validate(
            UserDto user,
            SalesforceObjectMetadata metadata)
        {
            var result =
                new ValidationResult();

            ValidateField(
                metadata,
                "ExternalId__c",
                36,
                result);

            ValidateField(
                metadata,
                "FirstName__c",
                100,
                result);

            ValidateField(
                metadata,
                "LastName__c",
                100,
                result);

            ValidateField(
                metadata,
                "Email__c",
                255,
                result);

            ValidateField(
                metadata,
                "Phone__c",
                50,
                result);

            return result;
        }

        private static void ValidateField(
            SalesforceObjectMetadata metadata,
            string fieldName,
            int expectedLength,
            ValidationResult result)
        {
            var field =
                metadata.fields.FirstOrDefault(
                    x => x.name == fieldName);

            if (field == null)
            {
                result.Errors.Add(
                    new ValidationError
                    {
                        FieldName = fieldName,
                        Message =
                            $"Field {fieldName} does not exist."
                    });

                return;
            }

            if (field.length < expectedLength)
            {
                result.Errors.Add(
                    new ValidationError
                    {
                        FieldName = fieldName,
                        Message =
                            $"Field length mismatch. Salesforce:{field.length} Expected:{expectedLength}"
                    });
            }
        }
    }
}