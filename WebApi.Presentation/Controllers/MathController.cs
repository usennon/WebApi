using Entities.TransferObjects;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using WebApi.Presentation.ActionFilters;

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
            return Ok(_service.MathService.GetSum(a, b));
        }

        [HttpGet("sub")]
        public IActionResult Sub([FromQuery] int a, [FromQuery] int b)
        {
            return Ok(_service.MathService.GetSub(a, b));
        }

        [HttpPost("sum")]
        public IActionResult GetSum([FromBody] NumberInputModel input)
        {
            var sum = _service.MathService.GetSum(input);
            return Ok(sum);
        }

        [HttpPost("average")]
        public IActionResult GetAverage([FromBody] NumberInputModel input)
        {
            if (input.Numbers.Count == 0)
            {
                return BadRequest("Input cannot be null");
            }
            var average = _service.MathService.GetAverage(input);
            return Ok(average);
        }

        [ServiceFilter(typeof(ValidateEvenPositiveNumberFilter))]
        [HttpPost("integral")]
        public IActionResult GetIntegral([FromBody] IntegralParametersModel input)
        {
            var result = _service.MathService.GetIntegral(input);
            return Ok(result);
        }

        [HttpPost("interest")]
        public IActionResult GetCompoundInterest([FromBody] CompoundInterstModel input)
        {
            var result = _service.MathService.GetCompoundInterest(input);
            return Ok(result);
        }



    }
}
