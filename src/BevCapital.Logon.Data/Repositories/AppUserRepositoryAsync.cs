using BevCapital.Logon.Data.Context;
using BevCapital.Logon.Domain.Entities;
using BevCapital.Logon.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.Logon.Data.Repositories
{
    public class AppUserRepositoryAsync : IAppUserRepositoryAsync
    {
        private readonly AppUserContext _appUserContext;

        public AppUserRepositoryAsync(AppUserContext appUserContext)
        {
            _appUserContext = appUserContext;
        }

        public async Task AddAsync(AppUser user, CancellationToken cancellationToken)
        {
            await _appUserContext.AppUsers.AddAsync(user, cancellationToken);
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _appUserContext.AppUsers.ToListAsync(cancellationToken);
        }

        public async Task<AppUser> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _appUserContext.AppUsers.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<AppUser> FindAsync(object[] keys, CancellationToken cancellationToken)
        {
            return await _appUserContext.AppUsers.FindAsync(keyValues: keys, cancellationToken: cancellationToken);
        }

        public void Remove(AppUser user)
        {
            _appUserContext.AppUsers.Remove(user);
        }
    }
}
