using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BevCapital.Logon.API.Controllers
{
    [Route("api/logon")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
