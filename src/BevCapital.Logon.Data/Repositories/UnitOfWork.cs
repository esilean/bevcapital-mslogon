using BevCapital.Logon.Data.Context;
using BevCapital.Logon.Domain.Repositories;
using System.Threading.Tasks;

namespace BevCapital.Logon.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppUserContext _appUserContext;

        public UnitOfWork(AppUserContext appUserContext)
        {
            _appUserContext = appUserContext;
            Users = new AppUserRepositoryAsync(_appUserContext);
        }

        public IAppUserRepositoryAsync Users { get; private set; }

        public async ValueTask DisposeAsync()
        {
            await _appUserContext.DisposeAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _appUserContext.SaveChangesAsync() > 0;
        }
    }
}
