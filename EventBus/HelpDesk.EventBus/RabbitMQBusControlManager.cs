using MassTransit;
using System;

namespace HelpDesk.EventBus
{
    public static class RabbitMQBusControlManager
    {

        private static Lazy<IBusControl> bus = new Lazy<IBusControl>(() =>
        {
            IBusControl bus = Bus.Factory.CreateUsingRabbitMq(x => x.Host(new Uri(_rabbitMQHost), h =>
            {
                h.Username(_userName);
                h.Password(_password);
            }));
            bus.Start();           
            return bus;
        });
        
        private static string _rabbitMQHost;
        private static string _userName;
        private static string _password;
        
        public static IBusControl GetBusControl(string rabbitMQHost, string userName, string password)
        {
            _rabbitMQHost = rabbitMQHost;
            _userName = userName;
            _password = password;

            return bus.Value;
        }

        public static void StopBus()
        {
            bus.Value.Stop();
        }
    }


}
