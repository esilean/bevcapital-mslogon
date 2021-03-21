using System;
using System.Threading.Tasks;

namespace BevCapital.Logon.Domain.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IAppUserRepositoryAsync Users { get; }

        Task<bool> SaveAsync();
    }
}
