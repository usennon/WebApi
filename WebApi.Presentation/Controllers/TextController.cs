using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Entities.TransferObjects;


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

        [HttpPost("concat")]
        public IActionResult Concatenate([FromBody] StringContainerModel strings)
        {
            return Ok(_service.TextService.Concatenate(strings.FirstString, strings.SecondString));
        }

        [HttpGet("clear")]
        public IActionResult Clear()
        {
            _service.TextService.Clear();
            return Ok();
        }

    }

}
