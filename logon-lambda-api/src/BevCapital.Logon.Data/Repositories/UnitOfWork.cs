using BevCapital.Logon.Data.Context.Interfaces;
using BevCapital.Logon.Domain.Core.Events;
using BevCapital.Logon.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace BevCapital.Logon.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IEventBus _eventBus;
        private readonly IDatabaseClient _client;

        public UnitOfWork(IEventBus eventBus,
                          IDatabaseClient client)
        {
            _eventBus = eventBus;
            _client = client;

            Users = new AppUserRepositoryAsync(_client);
        }

        public IAppUserRepositoryAsync Users { get; private set; }

        public async Task<bool> SaveAndCommitEvents(IEvent @event)
        {
            try
            {
                await _eventBus.Commit(@event);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
