using BevCapital.Logon.Domain.Core.Events;
using System;
using System.Threading.Tasks;

namespace BevCapital.Logon.Domain.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable, IDisposable
    {
        IAppUserRepositoryAsync Users { get; }

        Task<bool> SaveChangesAndCommitAsync(IEvent @event);
    }
}
