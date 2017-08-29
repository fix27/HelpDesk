using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Collections.Generic;

namespace HelpDesk.CabinetWebApp.Filters
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