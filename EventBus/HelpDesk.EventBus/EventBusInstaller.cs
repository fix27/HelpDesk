using HelpDesk.Common.EventBus.Interface;
using MassTransit;
using Microsoft.Practices.Unity;
using System;

namespace HelpDesk.EventBus
{
    public static class EventBusInstaller
    {
        public static void Install(IUnityContainer container)
        {
            container.RegisterType<IQueue, Queue>();
        }
    }
}
