using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.Common.EventBus.Interface;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace HelpDesk.EventBus
{
    public class Queue<T>: IQueue<T>
        where T : class, IAppEvent 
    {
        private readonly IRequestClient<T, T> client;
        private readonly IBusControl bus;

        public Queue(IRequestClient<T, T> client, IBusControl bus)
        {
            this.client = client;
            this.bus = bus;
        }
        
        public void Push(T evnt)
        {
            
            Task.Run(async () =>
            {
                T response = await client.Request(evnt);
            }).Wait();
            bus.Stop();
        }

        public void Stop()
        {
            bus.Stop();
        }
    }
}
