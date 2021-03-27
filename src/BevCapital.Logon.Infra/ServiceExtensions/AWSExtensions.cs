using Amazon.SimpleNotificationService;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using BevCapital.Logon.Infra.MessageBrokers.Aws;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BevCapital.Logon.Infra.ServiceExtensions
{
    public static class AWSExtensions
    {
        public static IServiceCollection AddAppAWS(this IServiceCollection services,
                                                           IConfiguration configuration,
                                                           IWebHostEnvironment environment)
        {
            services.Configure<SNSSettings>(configuration.GetSection("SNSSettings"));

            if (!environment.IsEnvironment("Testing"))
            {
                AWSXRayRecorder recorder = new AWSXRayRecorderBuilder().Build();
                AWSXRayRecorder.InitializeInstance(configuration, recorder);
                AWSSDKHandler.RegisterXRayForAllServices();
            }

            services.AddAWSService<IAmazonSimpleNotificationService>();

            return services;
        }

        public static IApplicationBuilder UseAWS(this IApplicationBuilder builder)
        {
            builder.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            builder.UseXRay("LogonApi");

            return builder;
        }
    }
}
