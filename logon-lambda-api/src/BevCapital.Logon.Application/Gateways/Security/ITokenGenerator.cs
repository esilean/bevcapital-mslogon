using BevCapital.Logon.Domain.Entities;
using System.Threading.Tasks;

namespace BevCapital.Logon.Application.Gateways.Security
{
    public interface ITokenGenerator
    {
        Task<string> CreateToken(AppUser appUser);

        string GenerateRefreshToken();
    }
}
