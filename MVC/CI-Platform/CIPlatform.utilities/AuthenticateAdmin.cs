using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace CIPlatform.utilities;

public class AuthenticateAdmin : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        string actionName = filterContext.ActionDescriptor.DisplayName;
        if (!actionName.Contains("Logout"))
        {
            string isAdmin = filterContext.HttpContext.Session.GetString("isAdmin");
            if (isAdmin == "true")
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                { "Controller", "Dashboard" },
                { "Action", "Index" },
                {"Area", "Admin" }
                });
            }
        }
    }
}
