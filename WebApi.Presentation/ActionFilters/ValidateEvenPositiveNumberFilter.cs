using Entities.TransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Presentation.ActionFilters
{
    public class ValidateEvenPositiveNumberFilter : IActionFilter
    {
        private readonly string _parameterName;

        public ValidateEvenPositiveNumberFilter(string parameterName)
        {
            _parameterName = parameterName;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue(_parameterName, out var value))
            {
                if (value is IntegralParametersModel input)
                {
                    if (input.IntervalsAmount <= 0 || input.IntervalsAmount % 2 != 0)
                    {
                        context.Result = new BadRequestObjectResult("The number must be a positive, even integer.");
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
