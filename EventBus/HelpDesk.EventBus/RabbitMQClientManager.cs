using HelpDesk.Common.EventBus.AppEvents.Interface;
using MassTransit;
using System;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace HelpDesk.EventBus
{
    class RabbitMQRequestClientManager<T>
        where T : class, IAppEvent
    {

        

        private static Lazy<IBusControl> bus = new Lazy<IBusControl>(() =>
        {
            IBusControl bus = Bus.Factory.CreateUsingRabbitMq(x => x.Host(new Uri(_rabbitMQHost), h =>
            {
                h.Username(_userName);
                h.Password(_password);
            }));
                       
            return bus;
        });

        private static Lazy<IRequestClient<T, T>> client = new Lazy<IRequestClient<T, T>>(() =>
        {
            IBusControl b = bus.Value;
            b.Start();
            var client = createRequestClient(b);
            return client;
        });

        private static IRequestClient<T, T> createRequestClient(IBusControl busControl)
        {
            var serviceUri = new Uri(_serviceAddress);
            var client = busControl.CreateRequestClient<T, T>(serviceUri, TimeSpan.FromSeconds(10));

            return client;
        }

        private static string _rabbitMQHost;
        private static string _serviceAddress;
        private static string _userName;
        private static string _password;
        public static IRequestClient<T, T> GetRequestClient(string rabbitMQHost, string serviceAddress, string userName, string password)
        {
            _rabbitMQHost = rabbitMQHost;
            _serviceAddress = serviceAddress;
            _userName = userName;
            _password = password;

            return client.Value;
        }

        public static IBusControl GetBusControl(string rabbitMQHost, string serviceAddress, string userName, string password)
        {
            _rabbitMQHost = rabbitMQHost;
            _serviceAddress = serviceAddress;
            _userName = userName;
            _password = password;

            return bus.Value;
        }


    }


}
