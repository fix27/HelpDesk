using System;
using HelpDesk.EventBus;
using Unity;
using HelpDesk.Data.NHibernate;
using Unity.Lifetime;
using MassTransit.Logging;
using Unity.Injection;
using HelpDesk.CalculateEventJob.Jobs;
using HelpDesk.Data.NHibernate.Repository;
using System.Configuration;

namespace HelpDesk.CalculateEventJob
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
            
            NHibernateDataInstaller.Install(container, new ContainerControlledLifetimeManager());
            NHibernateRepositoryInstaller.Install(container);

            container.RegisterType<ILog>(new InjectionFactory(c => Logger.Get<CalculateRequestDeedlineAppEventJob>()));
            //регистрация шины
            EventBusInstaller.Install(container,
                ConfigurationManager.AppSettings["RabbitMQHost"],
                ConfigurationManager.AppSettings["ServiceAddress"],
                ConfigurationManager.AppSettings["RabbitMQUserName"],
                ConfigurationManager.AppSettings["RabbitMQPassword"]);
           
        }
        
    }    
        
}
