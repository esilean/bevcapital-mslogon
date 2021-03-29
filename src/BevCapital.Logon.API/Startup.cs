using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Core.Internal.Entities;
using BevCapital.Logon.API.Middlewares;
using BevCapital.Logon.Data.Context;
using BevCapital.Logon.Infra.ServiceExtensions;
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
            services.AddSecrets(Configuration);

            services
                    .AddAppCore(Configuration)
                    .AddAppSwaggerLogon()
                    .AddAppSecurity()
                    .AddAppDistributedCache(Configuration)
                    .AddAppAWS(Configuration, HostingEnvironment)
                    .AddAppDatabase(Configuration)
                    .AddAppOutbox(Configuration)
                    .AddAppOutboxService(Configuration)
                    .AddAppMessageBrokers()
                    .AddAppHealthCheck(Configuration);
        }

        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              AppUserContext appUserContext,
                              OutboxContext outboxContext,
                              ILogger<Startup> logger)
        {
            Log.Information($"Hosting enviroment = {env.EnvironmentName}");
            Log.Information($"RDS_ENDPOINT = {Environment.GetEnvironmentVariable("RDS_ENDPOINT")}");

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

            Seed(appUserContext, outboxContext, logger);
        }

        private void Seed(AppUserContext appUserContext, OutboxContext outboxContext, ILogger<Startup> logger)
        {
            // XRAY - EFCore - AsyncLocal Problems
            string traceId = TraceId.NewId();
            AWSXRayRecorder.Instance.BeginSegment("DB Migration", traceId);
            try
            {
                logger.LogInformation("Initializing AppUserContext Database Migration.");
                appUserContext.Database.Migrate();
                logger.LogInformation("Finishing AppUserContext Database Migration...");
                logger.LogInformation("Initializing OutboxContext Database Migration.");
                outboxContext.Database.Migrate();
                logger.LogInformation("Finishing OutboxContext Database Migration...");

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
