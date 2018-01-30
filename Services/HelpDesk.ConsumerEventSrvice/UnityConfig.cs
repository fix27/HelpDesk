using HelpDesk.EventBus.Common.AppEvents.Interface;
using HelpDesk.ConsumerEventService.Consumers;
using HelpDesk.ConsumerEventService.EmailTemplateServices;
using HelpDesk.ConsumerEventService.Sender;
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
using System.Configuration;
using System.Collections.Generic;

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
            container.RegisterType<ISender, EmailSender>("EmailSender",
               new InjectionConstructor(
                   container.Resolve<IEmailTemplateService>(),
                   container.Resolve<ILog>("EmailSender")
                ));

            string oneSignalAppId = ConfigurationManager.AppSettings["OneSignal:AppId"];
            if (!String.IsNullOrWhiteSpace(oneSignalAppId))
                container.RegisterType<ISender, WebPushSender>("WebPushSender",
                   new InjectionConstructor(
                       oneSignalAppId,
                       container.Resolve<ILog>("WebPushSender")
                    ));

            IList<ISender> senders = new List<ISender>();
            senders.Add(container.Resolve<ISender>("EmailSender"));
            if (!String.IsNullOrWhiteSpace(oneSignalAppId))
                senders.Add(container.Resolve<ISender>("WebPushSender"));

            DataServiceCommonInstaller.Install(container);
            container.RegisterType<IConsumer<IRequestAppEvent> , RequestAppEventConsumer>(
                new InjectionConstructor(
                    container.Resolve<IQueryRunner>(),
                    container.Resolve<IStatusRequestMapService>(),
                    container.Resolve<ILog>("RequestAppEventConsumer"),
                    senders
                ));

            container.RegisterType<IConsumer<IRequestDeedlineAppEvent> , RequestDeedlineAppEventConsumer>(
                new InjectionConstructor(
                    container.Resolve<IQueryRunner>(),
                    container.Resolve<ILog>("RequestDeedlineAppEventConsumer"),
                    senders
                ));

            container.RegisterType<IConsumer<IUserPasswordRecoveryAppEvent> , UserPasswordRecoveryAppEventConsumer>(
               new InjectionConstructor(
                    container.Resolve<ILog>("UserPasswordRecoveryAppEventConsumer"),
                    senders[0]
                ));

            container.RegisterType<IConsumer<IUserRegisterAppEvent> , UserRegisterAppEventConsumer>(
                new InjectionConstructor(
                    container.Resolve<ILog>("UserRegisterAppEventConsumer"),
                    senders[0]
                ));
        }
        
    }    
        
}
