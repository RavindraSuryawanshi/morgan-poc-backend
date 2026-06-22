using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Morgan.Salesforce.POC.AzureFunctions.Models;
using Morgan.Salesforce.POC.AzureFunctions.Services;
using Morgan.SalesforceSyncPOC.AzureFunctions.Exceptions;
using Morgan.SalesforceSyncPOC.AzureFunctions.Services;
using Serilog.Context;
using System.Text.Json;

namespace Morgan.Salesforce.POC.AzureFunctions.Functions
{
    /// <summary>
    /// Processes user events from Service Bus and synchronizes data with Salesforce.
    /// </summary>
    public sealed class SalesforceSyncFunction
    {
        private readonly ILogger<SalesforceSyncFunction> _logger;
        private readonly IUserService _userService;
        private readonly ISalesforceMetadataService _metadataService;
        private readonly ISchemaValidator _schemaValidator;
        private readonly ISalesforceSyncService _syncService;


        public SalesforceSyncFunction(ILogger<SalesforceSyncFunction> logger, IUserService userService,
            ISalesforceMetadataService metadataService, ISchemaValidator schemaValidator, ISalesforceSyncService syncService)
        {
            _logger = logger;
            _userService = userService;
            _metadataService = metadataService;
            _schemaValidator = schemaValidator;
            _syncService = syncService;
        }

        /// <summary>
        /// Handles incoming Service Bus messages and performs Salesforce synchronization.
        /// </summary>
        [Function(nameof(SalesforceSyncFunction))]
        public async Task Run(
            [ServiceBusTrigger(
            topicName: "user-events",
            subscriptionName: "salesforce-sync",
            Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,

            ServiceBusMessageActions messageActions)
        {
            using (LogContext.PushProperty("ApplicationName", "SalesforceFunction"))
            using (LogContext.PushProperty("CorrelationId", message.CorrelationId))
            using (LogContext.PushProperty("MessageId", message.MessageId))
            {
                try
                {
                    _logger.LogInformation("Service Bus message received");
                    var payload = message.Body.ToString();

                    var @event = JsonSerializer.Deserialize<UserCreatedEvent>(payload);
                    _logger.LogInformation("Received Event. UserId: {UserId}", @event?.UserId);

                    if (@event is null)
                    {
                        _logger.LogError("Unable to deserialize UserCreatedEvent");

                        throw new InvalidOperationException("Invalid event payload.");
                    }

                    var user = await _userService.GetByIdAsync(@event!.UserId, CancellationToken.None);

                    if (user is null)
                    {
                        _logger.LogError("User not found. UserId: {UserId}",
                            @event.UserId);

                        throw new InvalidOperationException($"User {@event.UserId} not found.");
                    }

                    _logger.LogInformation("User Loaded. Id:{Id} Name:{FirstName}",
                        user?.Id,
                        user?.FirstName);


                    _logger.LogInformation("Loading Salesforce metadata");
                    var metadata = await _metadataService.GetObjectMetadataAsync(CancellationToken.None);
                    _logger.LogInformation("Salesforce Fields Count: {Count}", metadata.fields.Count);
                    
                    //foreach (var field in metadata.fields)
                    //{
                    //    _logger.LogInformation("{FieldName} {Type} {Length}",
                    //        field.name,
                    //        field.type,
                    //        field.length);
                    //}


                    var validationResult = _schemaValidator.Validate(user!, metadata);
                    //Log validation error, throw and exit
                    if (!validationResult.IsValid)
                    {
                        _logger.LogError("Salesforce schema validation failed");
                        foreach (var error in validationResult.Errors)
                        {
                            _logger.LogError("{Field} - {Message}", error.FieldName, error.Message);
                        }

                        throw new SchemaValidationException(validationResult.Errors);
                    }

                    //Success Log
                    _logger.LogInformation("Salesforce schema validation successful");

                    _logger.LogInformation("Salesforce synchronization started");
                    await _syncService.UpsertCustomerAsync(user!, CancellationToken.None);
                    _logger.LogInformation("Data synced successfully. UserId:{UserId}", user!.Id);

                    await messageActions.CompleteMessageAsync(message);
                    _logger.LogInformation("Service Bus message completed successfully");
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Salesforce synchronization failed");
                    throw;
                }
            }
        }
    }
}
