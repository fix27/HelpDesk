using HelpDesk.EventBus.Common.AppEvents.Interface;
using HelpDesk.ConsumerEventService.Consumers;
using HelpDesk.ConsumerEventService.EmailTemplateServices;
using HelpDesk.ConsumerEventService.Sender;
using HelpDesk.Data;
using HelpDesk.Data.NHibernate;
using HelpDesk.Data.NHibernate.Repository;
using HelpDesk.Data.Query;
using HelpDesk.DataService.Common;
using HelpDesk.DataService.Common.Interface;
using MassTransit;
using MassTransit.Logging;
using System;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using HelpDesk.Common.Cache;

namespace HelpDesk.ConsumerEventService
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
            CacheInstaller.Install(container);
            NHibernateDataInstaller.Install(container, new ContainerControlledLifetimeManager());
            NHibernateRepositoryInstaller.Install(container);
            DataServiceCommonInstaller.Install(container);
            //логеры
            container.RegisterType<ILog>("EmailSender", new InjectionFactory(c => Logger.Get<EmailSender>()));
            container.RegisterType<ILog>("RequestAppEventConsumer", new InjectionFactory(c => Logger.Get<RequestAppEventConsumer>()));
            container.RegisterType<ILog>("RequestDeedlineAppEventConsumer", new InjectionFactory(c => Logger.Get<RequestDeedlineAppEventConsumer>()));
            container.RegisterType<ILog>("UserPasswordRecoveryAppEventConsumer", new InjectionFactory(c => Logger.Get<UserPasswordRecoveryAppEventConsumer>()));
            container.RegisterType<ILog>("UserRegisterAppEventConsumer", new InjectionFactory(c => Logger.Get<UserRegisterAppEventConsumer>()));

            //шаблонизатор
            container.RegisterType<IEmailTemplateService, RazorEmailTemplateService>();
            
            //получатели сообщений из шины
            container.RegisterType<ISender, EmailSender>(
               new InjectionConstructor(
                   container.Resolve<IEmailTemplateService>(),
                   container.Resolve<ILog>("EmailSender")
                ));

            DataServiceCommonInstaller.Install(container);
            container.RegisterType<IConsumer<IRequestAppEvent> , RequestAppEventConsumer>(
                new InjectionConstructor(
                    container.Resolve<IQueryRunner>(),
                    container.Resolve<IStatusRequestMapService>(),
                    container.Resolve<ILog>("RequestAppEventConsumer"),
                    container.Resolve<ISender>()
                ));

            container.RegisterType<IConsumer<IRequestDeedlineAppEvent> , RequestDeedlineAppEventConsumer>(
                new InjectionConstructor(
                    container.Resolve<IQueryRunner>(),
                    container.Resolve<ILog>("RequestDeedlineAppEventConsumer"),
                    container.Resolve<ISender>()
                ));

            container.RegisterType<IConsumer<IUserPasswordRecoveryAppEvent> , UserPasswordRecoveryAppEventConsumer>(
               new InjectionConstructor(
                    container.Resolve<ILog>("UserPasswordRecoveryAppEventConsumer"),
                    container.Resolve<ISender>()
                ));

            container.RegisterType<IConsumer<IUserRegisterAppEvent> , UserRegisterAppEventConsumer>(
                new InjectionConstructor(
                    container.Resolve<ILog>("UserRegisterAppEventConsumer"),
                    container.Resolve<ISender>()
                ));
        }
        
    }    
        
}
