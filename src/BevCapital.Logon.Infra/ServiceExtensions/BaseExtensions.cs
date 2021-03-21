using AutoMapper;
using BevCapital.Logon.Application.UseCases.User;
using BevCapital.Logon.Domain.Notifications;
using BevCapital.Logon.Infra.Notifications;
using BevCapital.Logon.Infra.Security;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BevCapital.Logon.Infra.ServiceExtensions
{
    public static class BaseExtensions
    {
        public static IServiceCollection ConfigureCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(Create.Handler).Assembly);
            services.AddAutoMapper(typeof(Create.Handler).Assembly);

            services.AddScoped<IAppNotificationHandler, AppNotificationHandler>();

            services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));

            return services;
        }
    }
}
