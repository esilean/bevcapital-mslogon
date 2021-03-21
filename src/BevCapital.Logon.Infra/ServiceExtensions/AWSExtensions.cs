using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
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
        public static IServiceCollection ConfigureAWS(this IServiceCollection services,
                                                           IConfiguration configuration,
                                                           IWebHostEnvironment environment)
        {
            if (!environment.IsEnvironment("Testing"))
            {
                AWSXRayRecorder recorder = new AWSXRayRecorderBuilder().Build();
                AWSXRayRecorder.InitializeInstance(configuration, recorder);
                AWSSDKHandler.RegisterXRayForAllServices();
            }

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
