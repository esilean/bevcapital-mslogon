using System;

namespace BevCapital.Logon.Domain.Core.Outbox
{
    public class OutboxMessage
    {
        public OutboxMessage()
        { }
        public OutboxMessage(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
        public string Type { get; set; }
        public string Data { get; set; }
        public DateTime? ProcessedAtUtc { get; set; }
    }
}
