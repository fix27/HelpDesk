using System;
using HelpDesk.EventBus;
using System.Web.Configuration;
using Unity;

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
           
            //регистрация шины
            EventBusInstaller.Install(container,
                WebConfigurationManager.AppSettings["RabbitMQHost"],
                WebConfigurationManager.AppSettings["ServiceAddress"],
                WebConfigurationManager.AppSettings["RabbitMQUserName"],
                WebConfigurationManager.AppSettings["RabbitMQPassword"]);
           
        }
        
    }    
        
}
