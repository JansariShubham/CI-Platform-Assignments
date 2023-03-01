using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CIPlatform.utilities
{
    public class Authentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetString("email") == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                        { "Controller", "Home" },
                           { "Action", "Login" }
                });
            }
        }
    }
}
