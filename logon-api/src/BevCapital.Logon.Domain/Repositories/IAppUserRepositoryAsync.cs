using BevCapital.Logon.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.Logon.Domain.Repositories
{
    public interface IAppUserRepositoryAsync
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AppUser>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AppUser> FindAsync(object[] keys, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<AppUser> GetByEmailAsync(string email, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task AddAsync(AppUser user, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        void Remove(AppUser user);
    }
}
