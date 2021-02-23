using Portal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace Portal.Filters
{
    public class UserAuthenticationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            Controller controller = filterContext.Controller as Controller;

            if (controller != null) 
            {
                if (session[SessionKey.CurrentUserID] == null)
                {
                    filterContext.HttpContext.Response.Redirect("~/Home/Index", true);
                }
            }

            base.OnActionExecuting(filterContext);
        }

    }

    //public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
    //{
    //    //We are checking Result is null or Result is HttpUnauthorizedResult 
    //    // if yes then we are Redirect to Error View
    //    if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
    //    {
    //        filterContext.Result = new ViewResult
    //        {
    //            ViewName = "Error"
    //        };
    //    }
    //}
}
