using AutoMapper;
using BevCapital.Logon.Application.UseCases.User;
using BevCapital.Logon.Domain.Core.Events;
using BevCapital.Logon.Domain.Notifications;
using BevCapital.Logon.Infra.Events;
using BevCapital.Logon.Infra.Filters;
using BevCapital.Logon.Infra.Notifications;
using BevCapital.Logon.Infra.Security;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BevCapital.Logon.Infra.ServiceExtensions
{
    public static class CoreExtensions
    {
        public static IServiceCollection AddAppCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(opts =>
            {
                opts.Filters.Add<NotificationFilter>();
            })
            .AddNewtonsoftJson(opts =>
            {
                opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            })
            .AddFluentValidation(cfg =>
            {
                cfg.RegisterValidatorsFromAssemblyContaining<Create>();
            });

            services.AddMediatR(typeof(Create.Handler).Assembly);
            services.AddAutoMapper(typeof(Create.Handler).Assembly);

            services.AddScoped<IAppNotificationHandler, AppNotificationHandler>();

            services.AddScoped<IEventBus, EventBus>();

            services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));

            return services;
        }
    }
}
