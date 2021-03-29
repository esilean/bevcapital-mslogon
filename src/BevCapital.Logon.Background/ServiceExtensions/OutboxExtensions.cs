using BevCapital.Logon.Application.Gateways.Outbox;
using BevCapital.Logon.Background.Outbox;
using BevCapital.Logon.Domain.Outbox;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BevCapital.Logon.Infra.ServiceExtensions
{
    public static class OutboxExtensions
    {
        public static IServiceCollection AddAppOutboxService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OutboxSettings>(configuration.GetSection("OutboxSettings"));

            services.AddHostedService<OutboxProcessorBackgroundService>();

            return services;
        }
    }
}
