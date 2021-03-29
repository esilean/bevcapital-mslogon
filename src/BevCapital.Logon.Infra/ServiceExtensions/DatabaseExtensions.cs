using BevCapital.Logon.Data.Context;
using BevCapital.Logon.Data.Repositories;
using BevCapital.Logon.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BevCapital.Logon.Infra.ServiceExtensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddAppDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppUserContext>(opts =>
            {
                opts.UseMySql(configuration.GetConnectionString("SqlCNN"));
                opts.AddXRayInterceptor(true);
            });

            services.AddScoped<IAppUserRepositoryAsync, AppUserRepositoryAsync>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<AppUserContext>();

            return services;
        }
    }
}
