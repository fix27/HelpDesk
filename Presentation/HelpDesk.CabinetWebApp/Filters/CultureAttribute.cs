using Microsoft.Practices.Unity;
using HelpDesk.DataService.Interface;
using HelpDesk.CabinetWebApp.App_Start;
using System.Collections.Generic;
using System.Web.Mvc;
using HelpDesk.CabinetWebApp.Helpers;

namespace HelpDesk.CabinetWebApp.Filters
{
    public class CultureAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            IDictionary<string, object> values = filterContext.RouteData.Values;

            string cultureName = null;
            CultureHelper.SetCulture(values, ref cultureName);

            filterContext.Controller.ViewBag.CurrentCulture = cultureName;

            UnityServiceLocator serviceLocator = new UnityServiceLocator(UnityConfig.GetConfiguredContainer());
            ICultureService cultureService = serviceLocator.GetInstance<ICultureService>();

            filterContext.Controller.ViewBag.Cultures = cultureService.GetList();

            serviceLocator = new UnityServiceLocator(UnityConfig.GetConfiguredContainer());
            JsResourceHelper jsResourceHelper = serviceLocator.GetInstance<JsResourceHelper>();
            jsResourceHelper.Set();
        }
    }
}