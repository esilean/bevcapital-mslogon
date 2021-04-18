using BevCapital.Logon.Application.Gateways.Outbox;
using BevCapital.Logon.Domain.Core.Events;
using System.Threading.Tasks;

namespace BevCapital.Logon.Infra.Events
{
    public class EventBus : IEventBus
    {
        private readonly IOutboxListener _outboxListener;

        public EventBus(IOutboxListener outboxListener)
        {
            _outboxListener = outboxListener;
        }

        public virtual async Task Commit(params IEvent[] events)
        {
            foreach (var @event in events)
            {
                await SendToMessageBroker(@event);
            }
        }

        private async Task SendToMessageBroker(IEvent @event)
        {
            await _outboxListener.Commit(@event);
        }
    }
}
