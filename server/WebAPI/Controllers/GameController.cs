using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Controllers.Base;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : BaseController
    {
        private readonly ILogger<GameController> _logger;

        public GameController(ILogger<GameController> logger, IMediator mediator) : base(mediator)
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
