using BevCapital.Logon.Infra.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace BevCapital.Logon.Infra.ServiceExtensions
{

    public static class SwaggerExtensions
    {
        public static IServiceCollection AddAppSwaggerLogon(this IServiceCollection services)
        {
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Logon API",
                    Description = "Handle authentication",
                    Contact = new OpenApiContact
                    {
                        Name = "le.bevilaqua@gmail.com",
                    }
                });

                opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into this field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                opts.OperationFilter<AuthOperationFilter>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opts.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerLogon(this IApplicationBuilder builder)
        {
            builder.UseSwagger();
            builder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Logon Api V1");
                c.RoutePrefix = "swagger";
            });

            return builder;
        }
    }
}
