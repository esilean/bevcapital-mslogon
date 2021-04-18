using BevCapital.Logon.Application.Gateways.Events;
using BevCapital.Logon.Infra.MessageBrokers.Aws;
using Microsoft.Extensions.DependencyInjection;

namespace BevCapital.Logon.Infra.ServiceExtensions
{
    public static class MessageBrokersExtensions
    {
        public static IServiceCollection AddAppMessageBrokers(this IServiceCollection services)
        {
            services.AddSingleton<IEventListener, SNSListener>();

            return services;
        }
    }
}
