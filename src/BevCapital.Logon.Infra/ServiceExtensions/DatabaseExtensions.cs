using System;
using BevCapital.Logon.Data.Context;
using BevCapital.Logon.Data.Repositories;
using BevCapital.Logon.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BevCapital.Logon.Infra.ServiceExtensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddAppDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("SqlCNN");

            var rdsEndpoint = Environment.GetEnvironmentVariable("RDS_ENDPOINT");
            var rdsPassword = Environment.GetEnvironmentVariable("RDS_PASSWORD");
            connString.Replace("RDS_ENDPOINT", rdsEndpoint);
            connString.Replace("RDS_PASSWORD", rdsPassword);

            services.AddDbContext<AppUserContext>(opts =>
            {
                opts.UseMySql(connString);
                opts.AddXRayInterceptor(true);
            });

            services.AddScoped<IAppUserRepositoryAsync, AppUserRepositoryAsync>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<AppUserContext>();

            return services;
        }
    }
}
