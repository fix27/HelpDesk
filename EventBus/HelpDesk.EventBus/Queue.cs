using HelpDesk.Common.EventBus.AppEvents;
using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.Common.EventBus.Interface;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace HelpDesk.EventBus
{
    public class Queue<T>: IQueue<T>, IDisposable
        where T : class, IAppEvent 
    {
        private readonly IBusControl bus;
        private readonly string serviceAddress;
        private readonly string rabbitMQHost;
        private readonly string userName;
        private readonly string password;

        private readonly IRequestClient<T, T> client;

        public Queue(string rabbitMQHost, string serviceAddress, string userName, string password)
        {
            this.rabbitMQHost = rabbitMQHost;
            this.serviceAddress = serviceAddress;
            this.userName = userName;
            this.password = password;

            bus = CreateBus();
            bus.Start();

            client = CreateRequestClient(bus);
        }

        public void Dispose()
        {
            bus.Stop();
        }

        public void Push(T evnt)
        {
            Task.Run(async () =>
            {
                T response = await client.Request(evnt);
            }).Wait();
        }
        
        private IRequestClient<T, T> CreateRequestClient(IBusControl busControl)
        {
            var serviceUri = new Uri(serviceAddress);
            var client = busControl.CreateRequestClient<T, T>(serviceUri, TimeSpan.FromSeconds(10));

            return client;
        }

        private IBusControl CreateBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(x => x.Host(new Uri(rabbitMQHost), h =>
            {
                h.Username(userName);
                h.Password(password);
            }));
        }

    }
}
