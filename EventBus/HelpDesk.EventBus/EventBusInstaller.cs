﻿using HelpDesk.EventBus.Common.AppEvents.Interface;
using HelpDesk.EventBus.Common.Interface;
using MassTransit;
using Unity;
using Unity.Injection;

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
                    new ResolvedParameter<IBusControl>()));
            container.RegisterType<IQueue<IUserPasswordRecoveryAppEvent>, Queue<IUserPasswordRecoveryAppEvent>>(
                new InjectionConstructor(
                    new ResolvedParameter<IBusControl>()));
            container.RegisterType<IQueue<IUserRegisterAppEvent>, Queue<IUserRegisterAppEvent>>(
                new InjectionConstructor(
                    new ResolvedParameter<IBusControl>()));
            container.RegisterType<IQueue<IRequestDeedlineAppEvent>, Queue<IRequestDeedlineAppEvent>>(
                new InjectionConstructor(
                    new ResolvedParameter<IBusControl>()));
            
        }
    }
}
