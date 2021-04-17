using BevCapital.Logon.Data.Context;
using BevCapital.Logon.Domain.Core.Outbox;
using BevCapital.Logon.Domain.Outbox;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BevCapital.Logon.Data.Outbox
{
    public class OutboxStore : IOutboxStore
    {
        private readonly OutboxContext _outboxContext;

        public OutboxStore(OutboxContext outboxContext)
        {
            _outboxContext = outboxContext;
        }

        public async Task Add(OutboxMessage message)
        {
            await _outboxContext.OutboxMessages.AddAsync(message);
            await _outboxContext.SaveChangesAsync();
        }

        public async Task Delete(IEnumerable<Guid> ids)
        {
            var messages = ids.Select(id => new OutboxMessage(id));
            _outboxContext.OutboxMessages.RemoveRange(messages);

            await _outboxContext.SaveChangesAsync();
        }

        public async Task<OutboxMessage> GetMessage(Guid id)
        {
            return await _outboxContext.OutboxMessages
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Guid>> GetUnprocessedMessageIds()
        {
            return await _outboxContext.OutboxMessages
                                    .Where(x => x.ProcessedAtUtc == null)
                                    .Select(x => x.Id)
                                    .ToListAsync();
        }

        public async Task SetMessageToProcessed(Guid id)
        {
            var message = new OutboxMessage(id)
            {
                ProcessedAtUtc = DateTime.UtcNow
            };

            _outboxContext.OutboxMessages.Attach(message);
            _outboxContext.Entry(message).Property(p => p.ProcessedAtUtc).IsModified = true;

            await _outboxContext.SaveChangesAsync();

            //remove from context
            _outboxContext.Entry(message).State = EntityState.Detached;
        }
    }
}
