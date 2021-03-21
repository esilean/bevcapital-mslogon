using BevCapital.Logon.Application.Gateways.Security;
using BevCapital.Logon.Domain.Entities;
using BevCapital.Logon.Infra.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BevCapital.Logon.Infra.ServiceExtensions
{
    public static class SecurityExtensions
    {
        public static IServiceCollection ConfigureSecurity(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("WWW-Authenticate")
                    .AllowCredentials();
                });
            });

            services.AddTransient<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddSingleton<ITokenSecret, TokenSecret>();

            return services;
        }

        public static IApplicationBuilder UseSecurity(this IApplicationBuilder builder)
        {
            builder.UseXContentTypeOptions();
            builder.UseReferrerPolicy(opt => opt.NoReferrer());
            builder.UseXXssProtection(opt => opt.EnabledWithBlockMode());
            builder.UseXfo(opt => opt.Deny());

            builder.UseCors("CorsPolicy");

            return builder;
        }
    }
}
