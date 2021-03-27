using Amazon.XRay.Recorder.Core;
using BevCapital.Logon.Application.Gateways.Events;
using BevCapital.Logon.Domain.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.Logon.Background.Services
{
    public sealed class OutboxProcessorBackgroundService : BackgroundService
    {
        private readonly OutboxSettings _outboxSettings;
        private readonly IEventListener _eventListener;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;

        public OutboxProcessorBackgroundService(IServiceScopeFactory serviceScopeFactory,
                                                IEventListener eventListener,
                                                IOptions<OutboxSettings> options)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _eventListener = eventListener;
            _outboxSettings = options.Value;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(SendOutboxMessages, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        private void SendOutboxMessages(object state)
        {
            _ = Process();
        }

        public async Task Process()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var outboxStore = scope.ServiceProvider.GetRequiredService<IOutboxStore>();
                var publishedMessageIds = new List<Guid>();

                try
                {
                    AWSXRayRecorder.Instance.BeginSegment(nameof(OutboxProcessorBackgroundService));

                    var messageIds = await outboxStore.GetUnprocessedMessageIds();

                    foreach (var messageId in messageIds)
                    {
                        var message = await outboxStore.GetMessage(messageId);
                        if (message is null || message.Processed.HasValue)
                        {
                            continue;
                        }

                        var success = await _eventListener.Publish(message.Id, message.Data);
                        if (success)
                        {
                            await outboxStore.SetMessageToProcessed(message.Id);
                            publishedMessageIds.Add(message.Id);
                        }
                    }
                }
                catch (Exception e)
                {
                    AWSXRayRecorder.Instance.AddException(e);
                }
                finally
                {
                    if (_outboxSettings.DeleteAfter)
                        await outboxStore.Delete(publishedMessageIds);

                    AWSXRayRecorder.Instance.EndSegment();
                }
            }
        }
    }
}
