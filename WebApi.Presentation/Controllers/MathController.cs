using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace WebApi.Presentation.Controllers
{
    [Route("api/math")]
    [ApiController]
    public class MathController : ControllerBase
    {
        private readonly IServiceManager _service;

        public MathController(IServiceManager service) => _service = service;

        [HttpGet("sum")]
        public IActionResult Sum([FromQuery] int a, [FromQuery] int b)
        {
            return Ok(_service.MathService.Sum(a, b));
        }

        [HttpGet("sub")]
        public IActionResult Sub([FromQuery] int a, [FromQuery] int b)
        {
            return Ok(_service.MathService.Sub(a, b));
        }
    }
}
