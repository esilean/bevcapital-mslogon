using BevCapital.Logon.Application.Gateways.Outbox;
using BevCapital.Logon.Data.Outbox;
using BevCapital.Logon.Domain.Outbox;
using BevCapital.Logon.Infra.Outbox;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BevCapital.Logon.Infra.ServiceExtensions
{
    public static class OutboxExtensions
    {
        public static IServiceCollection AddAppOutbox(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOutboxStore, OutboxStore>();
            services.AddScoped<IOutboxListener, OutboxListener>();

            return services;
        }
    }
}
