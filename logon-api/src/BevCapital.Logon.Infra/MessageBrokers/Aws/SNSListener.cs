using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using BevCapital.Logon.Application.Gateways.Events;
using BevCapital.Logon.Domain.Core.Outbox;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BevCapital.Logon.Infra.MessageBrokers.Aws
{
    public class SNSListener : IEventListener
    {
        private readonly ILogger<SNSListener> _logger;
        private readonly IAmazonSimpleNotificationService _amazonSimpleNotificationService;
        private readonly SNSSettings _snsSettings;

        public SNSListener(IAmazonSimpleNotificationService amazonSimpleNotificationService,
                           IOptions<SNSSettings> options,
                           ILogger<SNSListener> logger)
        {
            _amazonSimpleNotificationService = amazonSimpleNotificationService;
            _snsSettings = options.Value;
            _logger = logger;
        }

        public async Task<bool> Publish(OutboxMessage message)
        {
            try
            {
                var request = new PublishRequest
                {
                    MessageGroupId = message.Id.ToString(),
                    MessageDeduplicationId = message.Id.ToString(),
                    Message = message.Data,
                    TopicArn = _snsSettings.TopicArn,
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>
                    {
                        { "message-type", CreateStringMessageAttr(message.Type) }
                    },
                };

                var response = await _amazonSimpleNotificationService.PublishAsync(request);

                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                //Publish to a DeadLetter?
                return false;
            }
        }

        private MessageAttributeValue CreateStringMessageAttr(string value)
        {
            var messageAttrValue = new MessageAttributeValue
            {
                DataType = "String",
                StringValue = value
            };

            return messageAttrValue;
        }
    }
}
