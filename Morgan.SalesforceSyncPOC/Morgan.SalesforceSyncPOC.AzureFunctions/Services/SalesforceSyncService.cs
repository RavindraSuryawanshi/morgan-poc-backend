using global::Morgan.Salesforce.POC.AzureFunctions.Settings;
using global::Morgan.SalesforceSyncPOC.AzureFunctions.Models;
using global::Morgan.SalesforceSyncPOC.AzureFunctions.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Morgan.Salesforce.POC.AzureFunctions.Services
{
    public sealed class SalesforceSyncService : ISalesforceSyncService
    {
        private readonly HttpClient _httpClient;
        private readonly ISalesforceAuthService _authService;
        private readonly SalesforceSettings _settings;
        private readonly ILogger<SalesforceSyncService> _logger;

        public SalesforceSyncService(HttpClient httpClient, ISalesforceAuthService authService, IOptions<SalesforceSettings> options, ILogger<SalesforceSyncService> logger)
        {
            _httpClient = httpClient;
            _authService = authService;
            _settings = options.Value;
            _logger = logger;
        }

        public async Task UpsertCustomerAsync(UserDto user, CancellationToken cancellationToken)
        {
            var token = await _authService.AuthenticateAsync(cancellationToken);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
            var payload = new SalesforceCustomerRequest
            {
                FirstName__c = user.FirstName,
                LastName__c = user.LastName,
                Email__c = user.Email,
                Phone__c = user.Phone
            };
            var url = $"{token.instance_url}/services/data/{_settings.ApiVersion}/sobjects/{_settings.ObjectName}/ExternalId__c/{user.ExternalId}";

            var response = await _httpClient.PatchAsJsonAsync(url, payload, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
{
                _logger.LogError("Salesforce upsert failed. StatusCode: {StatusCode}. Response: {Response}",
                    response.StatusCode,
                    responseContent);

                return;
            }
        }
    }
}