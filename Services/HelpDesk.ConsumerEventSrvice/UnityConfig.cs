using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.ConsumerEventSrvice.Consumers;
using HelpDesk.ConsumerEventSrvice.EmailTemplateServices;
using HelpDesk.ConsumerEventSrvice.Sender;
using HelpDesk.Data;
using HelpDesk.Data.NHibernate;
using HelpDesk.Data.NHibernate.Repository;
using HelpDesk.Data.Query;
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
            DataInstaller.Install(container, new ContainerControlledLifetimeManager());
            NHibernateDataInstaller.Install(container, new ContainerControlledLifetimeManager());
            NHibernateRepositoryInstaller.Install(container);
            
            container.RegisterType<IEmailTemplateService, RazorEmailTemplateService>();
            container.RegisterType<ISender, EmailSender>(); 

            container.RegisterType<ILog>("RequestAppEventConsumer", new InjectionFactory(c => Logger.Get<RequestAppEventConsumer>()));
            container.RegisterType<ILog>("RequestDeedlineAppEventConsumer", new InjectionFactory(c => Logger.Get<RequestDeedlineAppEventConsumer>()));
            container.RegisterType<ILog>("UserPasswordRecoveryAppEventConsumer", new InjectionFactory(c => Logger.Get<UserPasswordRecoveryAppEventConsumer>()));
            container.RegisterType<ILog>("UserRegisterAppEventConsumer", new InjectionFactory(c => Logger.Get<UserRegisterAppEventConsumer>()));

            //IQueryRunner queryRunner, ILog log, ISender sender
            container.RegisterType<IConsumer<IRequestAppEvent> , RequestAppEventConsumer>(
                new InjectionConstructor(
                    container.Resolve<IQueryRunner>(),
                    container.Resolve<ILog>("RequestAppEventConsumer"),
                    container.Resolve<ISender>()
                ));

            //IQueryRunner queryRunner, ILog log, ISender sender
            container.RegisterType<IConsumer<IRequestDeedlineAppEvent> , RequestDeedlineAppEventConsumer>(
                new InjectionConstructor(
                    container.Resolve<IQueryRunner>(),
                    container.Resolve<ILog>("RequestDeedlineAppEventConsumer"),
                    container.Resolve<ISender>()
                ));

            //ILog log, ISender sender
            container.RegisterType<IConsumer<IUserPasswordRecoveryAppEvent> , UserPasswordRecoveryAppEventConsumer>(
               new InjectionConstructor(
                    container.Resolve<ILog>("UserPasswordRecoveryAppEventConsumer"),
                    container.Resolve<ISender>()
                ));

            //ILog log, ISender sender
            container.RegisterType<IConsumer<IUserRegisterAppEvent> , UserRegisterAppEventConsumer>(
                new InjectionConstructor(
                    container.Resolve<ILog>("UserRegisterAppEventConsumer"),
                    container.Resolve<ISender>()
                ));
        }
        
    }    
        
}
