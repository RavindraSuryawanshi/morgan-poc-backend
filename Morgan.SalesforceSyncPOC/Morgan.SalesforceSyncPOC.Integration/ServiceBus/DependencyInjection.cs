using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Morgan.SalesforceSyncPOC.Application.Interfaces;

namespace Morgan.SalesforceSyncPOC.Integration.ServiceBus
{
    /// <summary>
    /// Registers Service Bus services and dependencies.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceBus(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<ServiceBusSettings>(
                configuration.GetSection("ServiceBus"));

            var connectionString =
                configuration["ServiceBusConnectionString"];

            services.AddSingleton(
                new ServiceBusClient(connectionString!));

            services.AddScoped<
                IEventPublisher,
                ServiceBusPublisher>();

            return services;
        }
    }
}
