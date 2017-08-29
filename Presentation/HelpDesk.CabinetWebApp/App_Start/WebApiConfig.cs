using Microsoft.Practices.Unity;
using System.Linq;
using System.Web.Http;
using HelpDesk.CabinetWebApp.App_Start;
using HelpDesk.DataService.Interface;
using System.Collections.Generic;
using System;

namespace HelpDesk.CabinetWebApp
{
    public static class WebApiConfig
    {
        // Web API configuration and services
        public static void Register(HttpConfiguration config)
        {
            //Unity configuration for Web API controllers
            var container = UnityConfig.GetConfiguredContainer();
            config.DependencyResolver = new UnityResolver(container);
            
            UnityServiceLocator serviceLocator = new UnityServiceLocator(container);
            ICultureService cultureService = serviceLocator.GetInstance<ICultureService>();
            IReadOnlyList<string> cultures = cultureService.GetList();

            // Web API routes
            config.MapHttpAttributeRoutes();
                        

            config.Routes.MapHttpRoute(
                name: "LangApi",
                routeTemplate: "api/{lang}/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { lang = String.Join("|", cultures) }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, lang = cultures[0] }
            );

            config.Routes.MapHttpRoute(
                name: "UploadFileApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

        }
    }
}