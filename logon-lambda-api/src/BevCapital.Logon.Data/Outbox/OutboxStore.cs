using Amazon.DynamoDBv2.Model;
using BevCapital.Logon.Data.Context.Interfaces;
using BevCapital.Logon.Domain.Constants;
using BevCapital.Logon.Domain.Core.Outbox;
using BevCapital.Logon.Domain.Outbox;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.Logon.Data.Outbox
{
    public class OutboxStore : IOutboxStore
    {
        private readonly IDatabaseClient _client;

        public OutboxStore(IDatabaseClient client)
        {
            _client = client;
        }

        public async Task Add(OutboxMessage message)
        {
            var createdAtUtc = new DateTimeOffset(message.CreatedAtUtc).ToUnixTimeSeconds();
            var timeToLiveUtc = new DateTimeOffset(DateTime.UtcNow.AddMinutes(5)).ToUnixTimeSeconds();

            var request = new PutItemRequest
            {
                TableName = DynamoTables.LOGON_OUTBOXMESSAGES,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = message.Id.ToString() } },
                    { "Type", new AttributeValue { S = message.Type } },
                    { "Data", new AttributeValue { S = message.Data } },
                    { "CreatedAtUtc", new AttributeValue { N = createdAtUtc.ToString() } },
                    { "ProcessedAtUtc", new AttributeValue { NULL = true } },
                    { "TimeToLive", new AttributeValue { N = timeToLiveUtc.ToString() } },

                }
            };

            await _client.CreateAsync(request, CancellationToken.None);
        }

    }
}
