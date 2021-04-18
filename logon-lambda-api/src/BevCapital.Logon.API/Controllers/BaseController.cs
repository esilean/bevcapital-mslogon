using BevCapital.Logon.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BevCapital.Logon.API.Controllers
{
    [Route("lambda/ms-logon/api/logon")]
    [ApiController]
    [Produces(Common.APPLICATION_JSON)]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
