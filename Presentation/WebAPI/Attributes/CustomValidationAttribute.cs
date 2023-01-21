using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Application.Wrappers;

namespace WebApi.Attributes;

public class CustomValidationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage)
                    .Distinct()
                    .ToList();

            if (errors.Count > 0)
            {
                var ex = new Application.Exceptions.ValidationCustomException(errors);
                var responseModel = new Response<string>() { Succeeded = false, Message = ex.Message, Errors = ex.Errors };
                context.Result = new JsonResult(responseModel) { StatusCode = 400 };
            }
        }
    }

}