using HelpDesk.Common.EventBus.AppEvents;
using HelpDesk.Common.EventBus.Interface;
using MassTransit;
using System;

namespace HelpDesk.EventBus
{
    public class Queue: IQueue, IDisposable
    {
        private IBusControl bus;
        public Queue()
        {
            bus = CreateBus();
            bus.Start();
        }

        public void Dispose()
        {
            bus.Stop();
        }

        public void Push(IAppEvent evnt)
        {
            var client = CreateRequestClient(bus);
            client.Request(evnt);
        }



        private IRequestClient<IAppEvent, IAppEvent> CreateRequestClient(IBusControl busControl)
        {
            var serviceAddress = new Uri("rabbitmq://localhost/HelpDesk");
            var client = busControl.CreateRequestClient<IAppEvent, IAppEvent>(serviceAddress, TimeSpan.FromSeconds(10));

            return client;
        }

        private IBusControl CreateBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(x => x.Host(new Uri("rabbitmq://localhost"), h =>
            {
                
            }));
        }

    }
}
