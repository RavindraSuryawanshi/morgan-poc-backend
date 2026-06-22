using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Morgan.SalesforceSyncPOC.Application.Interfaces;
using Morgan.SalesforceSyncPOC.Integration.ServiceBus;


namespace Morgan.SalesforceSyncPOC.Integration.Outbox
{
    /// <summary>
    /// Registers outbox services and dependencies.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddOutbox(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<OutboxPublisherOptions>(
                configuration.GetSection("OutboxPublisher"));

            services.Configure<ServiceBusSettings>(
                configuration.GetSection("ServiceBus"));

            var serviceBusConnectionString =
                configuration["ServiceBusConnectionString"];

            services.AddSingleton(
                new ServiceBusClient(serviceBusConnectionString!));

            services.AddScoped<
                IOutboxRepository,
                OutboxRepository>();

            services.AddScoped<
                IOutboxPublisher,
                OutboxPublisher>();

            services.AddHostedService<
                OutboxPublisherHostedService>();

            return services;
        }
    }
}
