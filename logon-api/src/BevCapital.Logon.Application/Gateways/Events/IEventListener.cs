using BevCapital.Logon.Domain.Core.Outbox;
using System.Threading.Tasks;

namespace BevCapital.Logon.Application.Gateways.Events
{
    public interface IEventListener
    {
        Task<bool> Publish(OutboxMessage message);
    }
}
