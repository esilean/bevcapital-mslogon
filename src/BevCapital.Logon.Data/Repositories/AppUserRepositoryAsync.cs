using BevCapital.Logon.Data.Context;
using BevCapital.Logon.Domain.Entities;
using BevCapital.Logon.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task AddAsync(AppUser user)
        {
            await _appUserContext.AppUsers.AddAsync(user);
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            return await _appUserContext.AppUsers.ToListAsync();
        }

        public async Task<AppUser> GetByEmailAsync(string email)
        {
            return await _appUserContext.AppUsers.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<AppUser> FindAsync<TId>(TId id)
        {
            return await _appUserContext.AppUsers.FindAsync(id);
        }

        public void Remove(AppUser user)
        {
            _appUserContext.AppUsers.Remove(user);
        }
    }
}
