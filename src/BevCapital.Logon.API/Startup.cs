using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Core.Internal.Entities;
using BevCapital.Logon.API.Filters;
using BevCapital.Logon.API.Middlewares;
using BevCapital.Logon.Application.UseCases.User;
using BevCapital.Logon.Data.Context;
using BevCapital.Logon.Infra.ServiceExtensions;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Text.Json;

namespace BevCapital.Logon.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opts =>
            {
                opts.Filters.Add<NotificationFilter>();
            })
            .AddFluentValidation(cfg =>
            {
                cfg.RegisterValidatorsFromAssemblyContaining<Create>();
            });

            services
                    .ConfigureCommonServices(Configuration)
                    .ConfigureSwaggerLogon()
                    .ConfigureSecurity()
                    .ConfigureDistributedCache(Configuration)
                    .ConfigureAWS(Configuration, HostingEnvironment)
                    .ConfigureDatabase(Configuration)
                    .ConfigureHealthCheck(Configuration);
        }

        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              AppUserContext context,
                              ILogger<Startup> logger)
        {
            Log.Information($"Hosting enviroment = {env.EnvironmentName}");

            app.UseMiddleware<ErrorHandlerMiddleware>();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerLogon();
            app.UseAWS();
            app.UseSecurity();
            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = reg => reg.Tags.Contains("ready"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.Map("/", async (context) =>
                {
                    var result = JsonSerializer.Serialize(new
                    {
                        machineName = Environment.MachineName,
                        appName = env.ApplicationName,
                        environment = env.EnvironmentName
                    });

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(result.ToString());
                });
            });

            Seed(context, logger);
        }

        private void Seed(AppUserContext context, ILogger<Startup> logger)
        {
            // XRAY - EFCore - AsyncLocal Problems
            String traceId = TraceId.NewId();
            AWSXRayRecorder.Instance.BeginSegment("DB Migration", traceId);
            try
            {
                logger.LogInformation("Initializing Database Migration.");
                context.Database.Migrate();
                logger.LogInformation("Finishing Database Migration...");
            }
            catch (Exception e)
            {
                AWSXRayRecorder.Instance.AddException(e);
            }
            finally
            {
                AWSXRayRecorder.Instance.EndSegment();
            }
        }


    }
}
