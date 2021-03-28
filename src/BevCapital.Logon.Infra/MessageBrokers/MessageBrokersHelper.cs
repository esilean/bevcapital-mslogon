using BevCapital.Logon.Domain.Core.Events;
using System.Linq;

namespace BevCapital.Logon.Infra.MessageBrokers
{
    public static class MessageBrokersHelper
    {
        public static string GetTypeName(IEvent @event)
        {
            return @event.GetType().FullName.Split('.').Last().ToLower();
        }
    }
}
