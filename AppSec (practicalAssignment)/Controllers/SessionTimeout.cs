using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppSec__practicalAssignment_.Controllers
{
    public class SessionTimeout : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("UserId")))
            {
                context.Result = new RedirectToPageResult("/Login");
            }
            base.OnActionExecuting(context);
        }
    }
}
