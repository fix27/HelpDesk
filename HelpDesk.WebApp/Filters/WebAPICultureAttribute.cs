using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using HelpDesk.WebApp.Helpers;
using HelpDesk.WebApp.App_Start;

namespace HelpDesk.WebApp.Filters
{
    public class WebAPICultureAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            
        }

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            IDictionary<string, object> values = filterContext.Request.GetRouteData().Values;

            string cultureName = null;
            CultureHelper.SetCulture(values, ref cultureName);

            
        }
    }
}