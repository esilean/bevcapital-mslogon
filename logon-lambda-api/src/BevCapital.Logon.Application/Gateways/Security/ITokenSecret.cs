using System;
using System.Threading.Tasks;

namespace BevCapital.Logon.Application.Gateways.Security
{
    public interface ITokenSecret : IDisposable
    {
        Task<string> GetSecretAsync();
    }
}
