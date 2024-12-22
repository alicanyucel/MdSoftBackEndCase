using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MdSoftBackEndCase.Filters
{
    public class AdminAuthorizationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var userName = context.HttpContext.Request.Headers["Username"];
            var isAdmin = context.HttpContext.Request.Headers["Admin"] == "1";

            if (string.IsNullOrEmpty(userName) || !isAdmin)
            {
                context.Result = new UnauthorizedResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }

}
