using BevCapital.Logon.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BevCapital.Logon.Domain.Repositories
{
    public interface IAppUserRepositoryAsync
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AppUser>> GetAllAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AppUser> FindAsync<TId>(TId id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<AppUser> GetByEmailAsync(string email);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task AddAsync(AppUser user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        void Remove(AppUser user);
    }
}
