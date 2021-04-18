using BevCapital.Logon.Application.Gateways.Outbox;
using BevCapital.Logon.Data.Context;
using BevCapital.Logon.Data.Outbox;
using BevCapital.Logon.Domain.Outbox;
using BevCapital.Logon.Infra.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BevCapital.Logon.Infra.ServiceExtensions
{
    public static class OutboxExtensions
    {
        public static IServiceCollection AddAppOutbox(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OutboxContext>(opts =>
            {
                opts.UseMySql(configuration.GetConnectionString("SqlCNN"));
                opts.AddXRayInterceptor(true);
            });
            services.AddScoped<OutboxContext>();

            services.AddScoped<IOutboxStore, OutboxStore>();
            services.AddScoped<IOutboxListener, OutboxListener>();

            return services;
        }
    }
}
