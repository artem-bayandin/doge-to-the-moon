using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Controllers.Base;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger, IMediator mediator) : base(mediator)
        {
            _logger = logger;
        }

        //[HttpGet("")]
        //public async Task<IActionResult> GetAll()
        //{
        //    var result = await Mediator.Send(new ProductsQuery());
        //    return Ok(result);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] CreateProductCommand request)
        //{
        //    var data = await Mediator.Send(request);
        //    return Result(data);
        //}
    }
}
