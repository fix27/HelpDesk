using System;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using HelpDesk.Migration;
using HelpDesk.DataService;
using HelpDesk.Data.NHibernate.Repository;
using HelpDesk.Data;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using HelpDesk.Data.NHibernate;
using HelpDesk.WorkerWebApp.Helpers;
using HelpDesk.EventBus;
using System.Web.Configuration;
using Unity.AspNet.Mvc;
using Unity;
using Unity.Exceptions;

namespace HelpDesk.WorkerWebApp.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        

        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();

            //string connectionString = WebConfigurationManager.ConnectionStrings["cs"].ConnectionString;

            string hibernateCfgFileName = System.Web.Hosting.HostingEnvironment.MapPath("~/bin/hibernate.cfg.xml");
            XElement root = XElement.Load(hibernateCfgFileName);
            var nsMgr = new XmlNamespaceManager(new NameTable());
            nsMgr.AddNamespace("NC", "urn:nhibernate-configuration-2.2");
            string connectionString = root.XPathSelectElement("//NC:property[@name='connection.connection_string']", nsMgr).Value;

            //регистрация NHibernate-репозиториев
            NHibernateDataInstaller.Install(container, new PerRequestLifetimeManager());
            NHibernateRepositoryInstaller.Install(container);
           
            DataInstaller.Install(container, new PerRequestLifetimeManager());

            //регистрация Мигратора
            MigratorInstaller.Install(container, connectionString);

            //регистрация дата-сервисов
            DataServiceInstaller.Install(container);

            //регистрация шины
            EventBusInstaller.Install(container,
                WebConfigurationManager.AppSettings["RabbitMQHost"],
                WebConfigurationManager.AppSettings["ServiceAddress"],
                WebConfigurationManager.AppSettings["RabbitMQUserName"],
                WebConfigurationManager.AppSettings["RabbitMQPassword"]);
            
            container.RegisterType<JsResourceHelper>();
            
        }
        
    }

    //Resolver for Web API controllers
    public class UnityResolver : IDependencyResolver
    {
        protected IUnityContainer container;

        public UnityResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException ex)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new UnityResolver(child);
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }
        
}
