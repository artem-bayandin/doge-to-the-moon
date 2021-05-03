using Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebAPI.Controllers.Base;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        private readonly ILogger<AuthController> _logger;

        public TestController(ILogger<AuthController> logger, IMediator mediator) : base(mediator)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> Ping()
        {
            await Task.Delay(50);
            return Result(CommandResult<string>.Ok("hello"));
        }
    }
}
