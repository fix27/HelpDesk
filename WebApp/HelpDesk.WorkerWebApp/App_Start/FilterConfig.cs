using HelpDesk.WorkerWebApp.Filters;
using System.Web.Mvc;

namespace HelpDesk.WorkerWebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RecaptchaFilter());
        }
    }
}
