using Microsoft.AspNetCore.Mvc;
using Service.Contracts;


namespace WebApi.Presentation.Controllers
{
    [Route("api/text")]
    [ApiController]
    public class TextController : ControllerBase
    {
        private readonly IServiceManager _service;

        public TextController(IServiceManager service) => _service = service;

        [HttpGet("uppercase")]
        public IActionResult GetUpperCase([FromQuery] string input)
        {
            return Ok(_service.TextService.ToUpperCase(input));
        }

        [HttpGet("concat")]
        public IActionResult Concatenate([FromQuery] string str1, [FromQuery] string str2)
        {
            return Ok(_service.TextService.Concatenate(str1, str2));
        }

    }

}
