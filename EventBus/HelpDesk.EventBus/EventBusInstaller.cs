using HelpDesk.Common.EventBus.Interface;
using Microsoft.Practices.Unity;

namespace HelpDesk.EventBus
{
    public static class EventBusInstaller
    {
        public static void Install(IUnityContainer container, 
            string rabbitMQHost, string serviceAddress, string userName, string password)
        {
            container.RegisterType<IQueue, Queue>(
                new InjectionConstructor(rabbitMQHost, serviceAddress, userName, password));
        }
    }
}
