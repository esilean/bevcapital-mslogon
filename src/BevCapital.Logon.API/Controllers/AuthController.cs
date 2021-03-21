using BevCapital.Logon.Application.UseCases.Auth;
using BevCapital.Logon.Application.UseCases.Auth.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BevCapital.Logon.API.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(IMediator mediator) : base(mediator) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("signin")]
        public async Task<ActionResult<UserTokenOut>> SignIn(Login.Command command)
        {
            return await _mediator.Send(command);
        }

    }
}
