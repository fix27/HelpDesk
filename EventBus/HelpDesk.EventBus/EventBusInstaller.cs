using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.Common.EventBus.Interface;
using Microsoft.Practices.Unity;

namespace HelpDesk.EventBus
{
    public static class EventBusInstaller
    {
        public static void Install(IUnityContainer container, 
            string rabbitMQHost, string serviceAddress, string userName, string password)
        {
            container.RegisterType<IQueue<IRequestAppEvent>, Queue<IRequestAppEvent>>(
                new InjectionConstructor(rabbitMQHost, serviceAddress, userName, password));

            container.RegisterType<IQueue<IUserPasswordRecoveryAppEvent>, Queue<IUserPasswordRecoveryAppEvent>>(
                new InjectionConstructor(rabbitMQHost, serviceAddress, userName, password));

            container.RegisterType<IQueue<IUserRegisterAppEvent>, Queue<IUserRegisterAppEvent>>(
                new InjectionConstructor(rabbitMQHost, serviceAddress, userName, password));
        }
    }
}
