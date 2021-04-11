using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BevCapital.Logon.API.Controllers
{
    [Route("api/logon")]
    [ApiController]
    [Produces("application/json")]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
