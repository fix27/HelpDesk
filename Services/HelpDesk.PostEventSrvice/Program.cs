using HelpDesk.Common.EventBus.AppEvents;
using MassTransit;
using System.Threading.Tasks;
using MassTransit.Util;
using System;

namespace HelpDesk.PostEventSrvice
{
    class Program
    {
        static IBusControl bus;
        static void Main(string[] args)
        {
            bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(new Uri("rabbitmq://localhost/"), h =>
                {

                });

                sbc.ReceiveEndpoint(host, "HelpDesk", ep =>
                {
                    ep.Handler<RequestAppEvent>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received: {context.Message.RequestEventId}");
                    });
                });
            });
            bus.Start();

            Console.ReadKey();
        }        
        
    }
}
