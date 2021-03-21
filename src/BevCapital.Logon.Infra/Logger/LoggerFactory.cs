using Amazon;
using Amazon.CloudWatchLogs;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.AwsCloudWatch;
using System;

namespace BevCapital.Logon.Infra.Logger
{
    public static class LoggerFactory
    {
        public static LoggerConfiguration BuildEmptyLoggerConfiguration()
        {
            return new LoggerConfiguration();
        }

        public static ILogger CreateLogger(LoggerConfiguration loggerConfiguration, LogEventLevel minimumLogEventLevel)
        {
            return loggerConfiguration
                .MinimumLevel.Is(minimumLogEventLevel)
                .CreateLogger();
        }

        public static LoggerConfiguration AppendFileLogger(this LoggerConfiguration logger,
                                                           string logFilePath,
                                                           int retainedFileCountLimit = 14)
        {
            return logger
                .WriteTo.File(path: logFilePath,
                              rollingInterval: RollingInterval.Day,
                              formatter: new CompactJsonFormatter(),
                              retainedFileCountLimit: retainedFileCountLimit);
        }

        public static LoggerConfiguration AppendConsoleLogger(this LoggerConfiguration logger)
        {
            return logger.WriteTo.Console();
        }

        public static LoggerConfiguration AppendAwsCloudwatchLogger(this LoggerConfiguration logger,
                                                                    string logGroupName,
                                                                    string environmentName,
                                                                    LogEventLevel minimumLogEvent)
        {
            logGroupName = $"{logGroupName}/{environmentName}".ToLower();

            var formatter = new CompactJsonFormatter();
            var options = new CloudWatchSinkOptions
            {
                LogGroupName = logGroupName,
                TextFormatter = formatter,
                MinimumLogEventLevel = minimumLogEvent,
                BatchSizeLimit = 100,
                QueueSizeLimit = 10000,
                Period = TimeSpan.FromSeconds(10),
                CreateLogGroup = true,
                LogStreamNameProvider = new DefaultLogStreamProvider(),
                RetryAttempts = 5,
                LogGroupRetentionPolicy = LogGroupRetentionPolicy.OneDay
            };

            var client = new AmazonCloudWatchLogsClient(RegionEndpoint.SAEast1);

            return logger
                .WriteTo.AmazonCloudWatch(options, client);
        }
    }
}
