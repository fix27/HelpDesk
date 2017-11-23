using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.Common.EventBus.Interface;
using HelpDesk.EventBus;
using MassTransit;
using Microsoft.Practices.Unity;

namespace HelpDesk.EventBus
{
    public static class EventBusInstaller
    {
        public static void Install(IUnityContainer container, 
            string rabbitMQHost, string serviceAddress, string userName, string password)
        {

            container.RegisterType<IRequestClient<IRequestAppEvent, IRequestAppEvent>>(
                new InjectionFactory(c => RabbitMQRequestClientManager<IRequestAppEvent>
                    .GetRequestClient(rabbitMQHost, serviceAddress, userName, password)),
                new InjectionFactory(c => RabbitMQRequestClientManager<IRequestAppEvent>
                    .GetBusControl(rabbitMQHost, serviceAddress, userName, password)));
            container.RegisterType<IRequestClient<IUserPasswordRecoveryAppEvent, IUserPasswordRecoveryAppEvent>>(
                new InjectionFactory(c => RabbitMQRequestClientManager<IUserPasswordRecoveryAppEvent>
                    .GetRequestClient(rabbitMQHost, serviceAddress, userName, password)),
                new InjectionFactory(c => RabbitMQRequestClientManager<IUserPasswordRecoveryAppEvent>
                    .GetBusControl(rabbitMQHost, serviceAddress, userName, password)));
            container.RegisterType<IRequestClient<IUserRegisterAppEvent, IUserRegisterAppEvent>>(
                new InjectionFactory(c => RabbitMQRequestClientManager<IUserRegisterAppEvent>
                    .GetRequestClient(rabbitMQHost, serviceAddress, userName, password)),
                new InjectionFactory(c => RabbitMQRequestClientManager<IUserRegisterAppEvent>
                    .GetBusControl(rabbitMQHost, serviceAddress, userName, password)));
            container.RegisterType<IRequestClient<IRequestDeedlineAppEvent, IRequestDeedlineAppEvent>>(
                new InjectionFactory(c => RabbitMQRequestClientManager<IRequestDeedlineAppEvent>
                    .GetRequestClient(rabbitMQHost, serviceAddress, userName, password)),
                new InjectionFactory(c => RabbitMQRequestClientManager<IRequestDeedlineAppEvent>
                    .GetBusControl(rabbitMQHost, serviceAddress, userName, password)));

            container.RegisterType<IQueue<IRequestAppEvent>, Queue<IRequestAppEvent>>();
            container.RegisterType<IQueue<IUserPasswordRecoveryAppEvent>, Queue<IUserPasswordRecoveryAppEvent>>();
            container.RegisterType<IQueue<IUserRegisterAppEvent>, Queue<IUserRegisterAppEvent>>();
            container.RegisterType<IQueue<IRequestDeedlineAppEvent>, Queue<IRequestDeedlineAppEvent>>();
            
        }
    }
}
