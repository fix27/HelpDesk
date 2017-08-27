using HelpDesk.DataService.Interface;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace HelpDesk.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes, ICultureService cultureService)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            IReadOnlyList<string> cultures = cultureService.GetList();

            routes.MapRoute(
                name: "Lang",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { controller = "AngularTemplate", action = "Index", id = UrlParameter.Optional },
                constraints: new { lang = String.Join("|",cultures) }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "AngularTemplate", action = "Index", id = UrlParameter.Optional, lang = cultures[0] }
            );
        }
    }
}
