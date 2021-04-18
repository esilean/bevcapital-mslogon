using BevCapital.Logon.Domain.Core.Outbox;
using System.Threading.Tasks;

namespace BevCapital.Logon.Domain.Outbox
{
    public interface IOutboxStore
    {
        Task Add(OutboxMessage message);
    }
}
