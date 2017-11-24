using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.Common.EventBus.Interface;
using MassTransit;
using MassTransit.Transports;
using System;
using System.Threading.Tasks;

namespace HelpDesk.EventBus
{
    public class Queue<T>: IQueue<T>
        where T : class, IAppEvent 
    {
        private readonly IBusControl bus;
        private readonly string serviceAddress;
        public Queue(IBusControl bus, string serviceAddress)
        {
            this.bus = bus;
            this.serviceAddress = serviceAddress;
        }

        //private IRequestClient<T, T> client = null;
        public void Push(T evnt)
        {
            /*if (client == null)
                client = bus.CreateRequestClient<T, T>(new Uri(serviceAddress), TimeSpan.FromSeconds(10)); ;
            Task.Run(async () =>
            {
                T response = await client.Request(evnt);
            }).Wait();*/

            bus.Publish<T>(evnt);
        }
    }
}
