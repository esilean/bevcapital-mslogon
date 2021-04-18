using BevCapital.Logon.Application.Gateways.Outbox;
using BevCapital.Logon.Domain.Core.Events;
using BevCapital.Logon.Domain.Core.Outbox;
using BevCapital.Logon.Domain.Outbox;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BevCapital.Logon.Infra.Outbox
{
    public class OutboxListener : IOutboxListener
    {
        private readonly IOutboxStore _outboxStore;

        public OutboxListener(IOutboxStore outboxStore)
        {
            _outboxStore = outboxStore;
        }

        public virtual async Task Commit<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var outboxMessage = new OutboxMessage
            {
                Type = EventTypeHelper.GetTypeName(@event.GetType()),
                Data = @event == null ? "{}" : JsonConvert.SerializeObject(@event, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
            };

            await Commit(outboxMessage);
        }

        public virtual async Task Commit(OutboxMessage message)
        {
            await _outboxStore.Add(message);
        }
    }
}
