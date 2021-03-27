using BevCapital.Logon.Application.UseCases.User;
using BevCapital.Logon.Application.UseCases.User.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BevCapital.Logon.API.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(IMediator mediator) : base(mediator) { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult<List<AppUserOut<Guid>>>> List()
        {
            return await _mediator.Send(new List.ListAppUserQuery { });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<ActionResult<AppUserOut<Guid>>> Detail(Guid id)
        {
            return await _mediator.Send(new Details.DetailAppUserQuery { Id = id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult<Unit>> Create(Create.CreateAppUserCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            await _mediator.Send(new Delete.DeleteAppUserCommand { Id = id });
            return NoContent();
        }
    }
}
