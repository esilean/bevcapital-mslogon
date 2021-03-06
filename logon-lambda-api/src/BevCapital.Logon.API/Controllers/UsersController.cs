using BevCapital.Logon.Application.UseCases.User;
using BevCapital.Logon.Application.UseCases.User.Response;
using BevCapital.Logon.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Http;
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
        /// list all users
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>A list of users</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AppUserOut>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<AppUserOut>>> List(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new List.ListAppUserQuery { }, cancellationToken);
        }

        /// <summary>
        /// gets the user by id
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The details of a user</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpGet("detail/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppUserOut))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(IReadOnlyCollection<Notification>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AppUserOut>> Detail(string email, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new Details.DetailAppUserQuery { Email = email }, cancellationToken);
        }

        /// <summary>
        /// creates an user
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Error</response>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Unit))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Unit>> Create(Create.CreateAppUserCommand command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        /// <summary>
        /// updates an user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpPut("update/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Unit))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(IReadOnlyCollection<Notification>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Unit>> Update(string email, Update.UpdateAppUserCommand command, CancellationToken cancellationToken)
        {
            command.Email = email;
            return await _mediator.Send(command, cancellationToken);
        }

        /// <summary>
        /// removes an user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="204">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Error</response>
        [HttpDelete("delete/{email}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Unit))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(IReadOnlyCollection<Notification>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Unit>> Delete(string email, CancellationToken cancellationToken)
        {
            await _mediator.Send(new Delete.DeleteAppUserCommand { Email = email }, cancellationToken);
            return NoContent();
        }
    }
}
