using BevCapital.Logon.Domain.Core.Events;
using BevCapital.Logon.Domain.Core.Outbox;
using System.Threading.Tasks;

namespace BevCapital.Logon.Application.Gateways.Outbox
{
    public interface IOutboxListener
    {
        Task Commit(OutboxMessage message);
        Task Commit<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
