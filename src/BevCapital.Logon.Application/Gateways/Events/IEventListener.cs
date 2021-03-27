using System;
using System.Threading.Tasks;

namespace BevCapital.Logon.Application.Gateways.Events
{
    public interface IEventListener
    {
        Task<bool> Publish(Guid messageId, string message);
    }
}
