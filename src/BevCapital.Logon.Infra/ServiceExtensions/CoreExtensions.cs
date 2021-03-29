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
using System;
using System.Collections.Generic;
using System.IO;

namespace BevCapital.Logon.Infra.ServiceExtensions
{
    public static class CoreExtensions
    {
        public static void AddSecrets(this IServiceCollection _, IConfiguration configuration)
        {
            var secretsJson = File.ReadAllText(@"appsecrets.json");
            var secrets = JsonConvert.DeserializeObject<IDictionary<string, string>>(secretsJson);

            foreach (var secret in secrets)
            {
                var values = secret.Value.Split("::");
                foreach (var value in values)
                {
                    configuration[secret.Key] = configuration[secret.Key]?.Replace(value, Environment.GetEnvironmentVariable(value));
                }
            }
        }


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
