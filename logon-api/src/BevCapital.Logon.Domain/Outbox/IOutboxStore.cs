using BevCapital.Logon.Domain.Core.Outbox;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BevCapital.Logon.Domain.Outbox
{
    public interface IOutboxStore
    {
        Task Add(OutboxMessage message);
        Task<IEnumerable<Guid>> GetUnprocessedMessageIds();
        Task SetMessageToProcessed(Guid id);
        Task Delete(IEnumerable<Guid> ids);
        Task<OutboxMessage> GetMessage(Guid id);
    }
}
