using Entities.TransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Presentation.ActionFilters
{
    public class ValidateEvenPositiveNumberFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("input", out var value))
            {
                if (value is IntegralParametersModel input)
                {
                    if (input.IntervalsAmount <= 0 || input.IntervalsAmount % 2 != 0)
                    {
                        context.Result = new BadRequestObjectResult(
                            $"The number must be a positive, even integer, given {input.IntervalsAmount}.");
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
