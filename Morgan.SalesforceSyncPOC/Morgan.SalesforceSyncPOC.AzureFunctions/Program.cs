using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Morgan.Salesforce.POC.AzureFunctions.Services;
using Morgan.Salesforce.POC.AzureFunctions.Settings;
using Morgan.SalesforceSyncPOC.AzureFunctions.Services;
using Morgan.SalesforceSyncPOC.Infrastrcture.Data;
using Serilog;

var kvClient = new SecretClient(
    new Uri(Environment.GetEnvironmentVariable("KeyVaultUrl")
        ?? "https://kv-morgan-poc-dev.vault.azure.net/"),
    new DefaultAzureCredential());

string sqlConnString;

try
{
    var secret = kvClient.GetSecret("SqlConnectionString");
    sqlConnString = secret?.Value?.Value;
}
catch (Azure.RequestFailedException ex) when (ex.Status == 404)
{
    throw new InvalidOperationException(
        "Critical configuration missing: 'SqlConnectionString1' was not found in Azure Key Vault or returned an empty value. " +
        "Application cannot start without a valid database connection string.");
}

if (string.IsNullOrWhiteSpace(sqlConnString))
{
    throw new InvalidOperationException(
        "Key Vault secret 'SqlConnectionString1' is empty. Application cannot start.");
}

var aiConnString = kvClient.GetSecret("ApplicationInsightsConnectionString").Value.Value;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        services.Configure<SalesforceSettings>(
            context.Configuration.GetSection("Salesforce"));

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(sqlConnString);
        });
        services.AddHttpClient();
        services.AddScoped<IUserService, UserService>();
        services.AddHttpClient<ISalesforceAuthService,SalesforceAuthService>();
        services.AddHttpClient<ISalesforceMetadataService,SalesforceMetadataService>();
        services.AddScoped<ISchemaValidator, SchemaValidator>();
        services.AddHttpClient<ISalesforceSyncService, SalesforceSyncService>();

    })
    .UseSerilog((context, services, configuration) =>
    {
        var appName =
            Environment.GetEnvironmentVariable(
                "ApplicationName")
            ?? "SalesforceFunction";

        var connectionString = aiConnString;

        var telemetryConfiguration =
            TelemetryConfiguration.CreateDefault();

        telemetryConfiguration.ConnectionString =
            connectionString;

        configuration
            .Enrich.FromLogContext()
            .Enrich.WithProperty(
                "ApplicationName",
                appName)
            .WriteTo.Console()
            .WriteTo.ApplicationInsights(
                telemetryConfiguration,
                TelemetryConverter.Traces);
    })
    .Build();



host.Run();