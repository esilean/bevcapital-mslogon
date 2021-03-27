using BevCapital.Logon.Data.Context;
using BevCapital.Logon.Domain.Core.Events;
using BevCapital.Logon.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace BevCapital.Logon.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IEventBus _eventBus;
        private readonly AppUserContext _appUserContext;

        public UnitOfWork(IEventBus eventBus,
                          AppUserContext appUserContext)
        {
            _eventBus = eventBus;
            _appUserContext = appUserContext;

            Users = new AppUserRepositoryAsync(_appUserContext);
        }

        public IAppUserRepositoryAsync Users { get; private set; }

        public async Task<bool> SaveChangesAndCommitAsync(IEvent @event)
        {
            try
            {
                using (var transaction = _appUserContext.Database.BeginTransaction())
                {
                    await _appUserContext.SaveChangesAsync();
                    await _eventBus.Commit(@event);
                    await transaction.CommitAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _appUserContext.DisposeAsync();
        }
    }
}
