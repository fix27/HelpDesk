using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.ConsumerEventSrvice.Consumers;
using HelpDesk.ConsumerEventSrvice.Consumers.Interface;
using HelpDesk.ConsumerEventSrvice.EmailTemplateServices;
using HelpDesk.ConsumerEventSrvice.Sender;
using HelpDesk.Data;
using HelpDesk.Data.NHibernate;
using HelpDesk.Data.NHibernate.Repository;
using MassTransit;
using MassTransit.Logging;
using System;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace HelpDesk.ConsumerEventSrvice
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
            DataInstaller.Install(container, new PerThreadLifetimeManager());
            NHibernateDataInstaller.Install(container, new PerThreadLifetimeManager());
            NHibernateRepositoryInstaller.Install(container);
            
            container.RegisterType<IEmailTemplateService, RazorEmailTemplateService>();
            container.RegisterType<ISender, EmailSender>(); 

            container.RegisterType<IRequestAppEventConsumerLog>(
                new InjectionFactory(c => Logger.Get<RequestAppEventConsumer>()));
            container.RegisterType<IRequestDeedlineAppEventConsumerLog>(
                new InjectionFactory(c => Logger.Get<RequestDeedlineAppEventConsumer>()));
            container.RegisterType<IUserPasswordRecoveryAppEventConsumerLog>(
                new InjectionFactory(c => Logger.Get<UserPasswordRecoveryAppEventConsumer>()));
            container.RegisterType<IUserRegisterAppEventConsumerLog>(
                new InjectionFactory(c => Logger.Get<UserRegisterAppEventConsumer>()));

            container.RegisterType<IConsumer<IRequestAppEvent> , RequestAppEventConsumer>();
            container.RegisterType<IConsumer<IRequestDeedlineAppEvent> , RequestDeedlineAppEventConsumer>();
            container.RegisterType<IConsumer<IUserPasswordRecoveryAppEvent> , UserPasswordRecoveryAppEventConsumer>();
            container.RegisterType<IConsumer<IUserRegisterAppEvent> , UserRegisterAppEventConsumer>();
        }
        
    }    
        
}
