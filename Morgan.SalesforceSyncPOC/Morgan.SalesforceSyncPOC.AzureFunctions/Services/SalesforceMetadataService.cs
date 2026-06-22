using global::Morgan.Salesforce.POC.AzureFunctions.Settings;
using global::Morgan.SalesforceSyncPOC.AzureFunctions.Models;
using global::Morgan.SalesforceSyncPOC.AzureFunctions.Services;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Morgan.Salesforce.POC.AzureFunctions.Services
{
    /// <summary>
    /// Provides Salesforce object metadata retrieval operations.
    /// </summary>
    public sealed class SalesforceMetadataService : ISalesforceMetadataService
    {
        private readonly HttpClient _httpClient;
        private readonly ISalesforceAuthService _authService;
        private readonly SalesforceSettings _settings;

        public SalesforceMetadataService(
            HttpClient httpClient,
            ISalesforceAuthService authService,
            IOptions<SalesforceSettings> options)
        {
            _httpClient = httpClient;
            _authService = authService;
            _settings = options.Value;
        }

        public async Task<SalesforceObjectMetadata>
            GetObjectMetadataAsync(
                CancellationToken cancellationToken)
        {
            var token =
                await _authService.AuthenticateAsync(
                    cancellationToken);

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token.access_token);

            var url =
                $"{token.instance_url}/services/data/{_settings.ApiVersion}/sobjects/{_settings.ObjectName}/describe";

            return await _httpClient
                .GetFromJsonAsync<SalesforceObjectMetadata>(
                    url,
                    cancellationToken)
                ?? throw new Exception("Metadata not found");
        }
    }
}