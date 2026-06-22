using global::Morgan.Salesforce.POC.AzureFunctions.Settings;
using global::Morgan.SalesforceSyncPOC.AzureFunctions.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Morgan.SalesforceSyncPOC.AzureFunctions.Services
{
    /// <summary>
    /// Provides authentication to Salesforce and returns access tokens.
    /// </summary>
    public sealed class SalesforceAuthService: ISalesforceAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly SalesforceSettings _settings;

        public SalesforceAuthService(
            HttpClient httpClient,
            IOptions<SalesforceSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<SalesforceTokenResponse>
            AuthenticateAsync(
                CancellationToken cancellationToken)
        {
            var values =
                new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["client_id"] = _settings.ClientId,
                    ["client_secret"] = _settings.ClientSecret,
                    ["username"] = _settings.Username,
                    ["password"] =
                        _settings.Password +
                        _settings.SecurityToken
                };

            using var content =
                new FormUrlEncodedContent(values);

            var response =
                await _httpClient.PostAsync(
                    $"{_settings.InstanceUrl}/services/oauth2/token",
                    content,
                    cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<SalesforceTokenResponse>(
                    cancellationToken: cancellationToken)
                ?? throw new Exception("Authentication failed");
        }
    }
}