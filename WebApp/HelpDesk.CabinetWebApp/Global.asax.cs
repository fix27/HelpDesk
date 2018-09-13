using log4net;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HelpDesk.CabinetWebApp.App_Start;
using HelpDesk.Migration;
using HelpDesk.DataService.Interface;
using Unity.ServiceLocation;

namespace HelpDesk.CabinetWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly ILog log = LogManager.GetLogger("HelpDesk.CabinetWebApp");
        
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            

            

            UnityServiceLocator serviceLocator = new UnityServiceLocator(UnityConfig.GetConfiguredContainer());
            ICultureService cultureService = serviceLocator.GetInstance<ICultureService>();
            ISettingsService settingsService = serviceLocator.GetInstance<ISettingsService>();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);



            RouteConfig.RegisterRoutes(RouteTable.Routes, cultureService);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
                       
            
            IMigrationRunner migrationRunner = serviceLocator.GetInstance<IMigrationRunner>();
            migrationRunner.Update();
            
        }
    }
}
