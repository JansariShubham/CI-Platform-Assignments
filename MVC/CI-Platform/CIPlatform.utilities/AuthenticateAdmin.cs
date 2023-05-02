using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace CIPlatform.utilities;

public class AuthenticateAdmin : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        string userName = filterContext.HttpContext.Session.GetString("firstName");
        string actionName = filterContext.ActionDescriptor.DisplayName;
        //if (userName is null)
        //{
        //    if (filterContext.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute)))
        //    {
        //        // Allow anonymous access to the action
        //        base.OnActionExecuting(filterContext);
        //        return;
        //    }
        //    //filterContext.Result = new RedirectToRouteResult(
        //    //new RouteValueDictionary { { "Controller", "Home" }, { "Action", "Login" }, { "Area", "Users" } });
        //}
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
