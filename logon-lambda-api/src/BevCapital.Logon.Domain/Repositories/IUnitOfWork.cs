using BevCapital.Logon.Domain.Core.Events;
using System.Threading.Tasks;

namespace BevCapital.Logon.Domain.Repositories
{
    public interface IUnitOfWork
    {
        IAppUserRepositoryAsync Users { get; }

        Task<bool> SaveAndCommitEvents(IEvent @event);
    }
}
