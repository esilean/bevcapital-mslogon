using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using BevCapital.Logon.Application.Gateways.Events;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BevCapital.Logon.Infra.MessageBrokers.Aws
{
    public class SNSListener : IEventListener
    {
        private readonly IAmazonSimpleNotificationService _amazonSimpleNotificationService;
        private readonly SNSSettings _snsSettings;

        public SNSListener(IAmazonSimpleNotificationService amazonSimpleNotificationService,
                           IOptions<SNSSettings> options)
        {
            _amazonSimpleNotificationService = amazonSimpleNotificationService;
            _snsSettings = options.Value;
        }

        public async Task<bool> Publish(Guid messageId, string message)
        {
            try
            {
                var request = new PublishRequest
                {
                    MessageGroupId = messageId.ToString(),
                    MessageDeduplicationId = messageId.ToString(),
                    Message = message,
                    TopicArn = _snsSettings.TopicArn,
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>
                    {
                        { "x-token", CreateStringMessageAttr("whatever") }
                    },
                };

                var response = await _amazonSimpleNotificationService.PublishAsync(request);

                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
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
