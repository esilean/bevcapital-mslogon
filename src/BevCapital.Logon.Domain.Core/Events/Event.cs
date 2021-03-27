using System;

namespace BevCapital.Logon.Domain.Core.Events
{
    public abstract class Event : IEvent
    {
        public virtual Guid Id { get; } = Guid.NewGuid();
        public virtual DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
    }
}
