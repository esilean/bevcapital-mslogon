using System.Threading.Tasks;

namespace BevCapital.Logon.Domain.Core.Events
{
    public interface IEventBus
    {
        Task Commit(params IEvent[] events);
    }
}
