using BevCapital.Logon.Application.UseCases.User;
using BevCapital.Logon.Application.UseCases.User.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BevCapital.Logon.API.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(IMediator mediator) : base(mediator) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ActionResult<List<AppUserOut<Guid>>>> List(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new List.ListAppUserQuery { }, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<ActionResult<AppUserOut<Guid>>> Detail(Guid id, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new Details.DetailAppUserQuery { Id = id }, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult<Unit>> Create(Create.CreateAppUserCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Unit>> Update(Guid id, Update.UpdateAppUserCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            return await _mediator.Send(command, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new Delete.DeleteAppUserCommand { Id = id }, cancellationToken);
            return NoContent();
        }
    }
}
