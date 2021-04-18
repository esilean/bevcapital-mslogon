using System;

namespace BevCapital.Logon.Application.UseCases.Auth.Response
{
    public class UserTokenOut
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
