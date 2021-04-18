using Amazon.DynamoDBv2;
using BevCapital.Logon.Data.Context;
using BevCapital.Logon.Data.Context.Interfaces;
using BevCapital.Logon.Data.Repositories;
using BevCapital.Logon.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BevCapital.Logon.Infra.ServiceExtensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddAppDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAmazonDynamoDB>(x => DynamoDbClientFactory.CreateClient());
            services.AddSingleton<IDatabaseClient, DatabaseClient>();

            services.AddScoped<IAppUserRepositoryAsync, AppUserRepositoryAsync>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
