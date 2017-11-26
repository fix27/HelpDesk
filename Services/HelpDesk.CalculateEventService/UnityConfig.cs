using System;
using HelpDesk.EventBus;
using System.Web.Configuration;
using Unity;
using HelpDesk.Data;
using HelpDesk.Data.NHibernate;
using Unity.Lifetime;
using MassTransit.Logging;
using Unity.Injection;
using HelpDesk.CalculateEventService.Jobs;
using HelpDesk.Data.NHibernate.Repository;

namespace HelpDesk.CalculateEventService
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

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        private static void RegisterTypes(IUnityContainer container)
        {
            DataInstaller.Install(container, new ContainerControlledLifetimeManager());
            NHibernateDataInstaller.Install(container, new ContainerControlledLifetimeManager());
            NHibernateRepositoryInstaller.Install(container);

            container.RegisterType<ILog>(new InjectionFactory(c => Logger.Get<CalculateRequestDeedlineAppEventJob>()));
            //регистрация шины
            EventBusInstaller.Install(container,
                WebConfigurationManager.AppSettings["RabbitMQHost"],
                WebConfigurationManager.AppSettings["ServiceAddress"],
                WebConfigurationManager.AppSettings["RabbitMQUserName"],
                WebConfigurationManager.AppSettings["RabbitMQPassword"]);
           
        }
        
    }    
        
}
