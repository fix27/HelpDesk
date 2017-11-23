using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.Common.EventBus.Interface;
using MassTransit;
using Microsoft.Practices.Unity;

namespace HelpDesk.EventBus
{
    public static class EventBusInstaller
    {
        public static void Install(IUnityContainer container, 
            string rabbitMQHost, string serviceAddress, string userName, string password)
        {

            container.RegisterType<IBusControl>(
                new InjectionFactory(c => RabbitMQBusControlManager.GetBusControl(rabbitMQHost, userName, password)));
            
            container.RegisterType<IQueue<IRequestAppEvent>, Queue<IRequestAppEvent>>(
                new InjectionConstructor(
                    new ResolvedParameter<IBusControl>(),
                    serviceAddress));
            container.RegisterType<IQueue<IUserPasswordRecoveryAppEvent>, Queue<IUserPasswordRecoveryAppEvent>>(
                new InjectionConstructor(
                    new ResolvedParameter<IBusControl>(),
                    serviceAddress));
            container.RegisterType<IQueue<IUserRegisterAppEvent>, Queue<IUserRegisterAppEvent>>(
                new InjectionConstructor(
                    new ResolvedParameter<IBusControl>(),
                    serviceAddress));
            container.RegisterType<IQueue<IRequestDeedlineAppEvent>, Queue<IRequestDeedlineAppEvent>>(
                new InjectionConstructor(
                    new ResolvedParameter<IBusControl>(),
                    serviceAddress));
            
        }
    }
}
