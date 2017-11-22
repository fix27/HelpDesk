using System;
using System.Configuration;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MassTransit.Util;
using Topshelf;
using Topshelf.Logging;

namespace HelpDesk.PostEventSrvice
{
    class RequestService: ServiceControl
    {
        readonly LogWriter log = HostLogger.Get<RequestService>();

        IBusControl busControl;

        public bool Start(HostControl hostControl)
        {
            log.Info("Creating bus...");

            busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                IRabbitMqHost host = x.Host(new Uri(ConfigurationManager.AppSettings["RabbitMQHost"]), h =>
                {
                    h.Username(ConfigurationManager.AppSettings["RabbitMQUserName"]);
                    h.Password(ConfigurationManager.AppSettings["RabbitMQPassword"]);
                });

                x.ReceiveEndpoint(host, ConfigurationManager.AppSettings["ServiceQueueName"],
                    e => { e.Consumer<RequestConsumer>(); });
            });

            log.Info("Starting bus...");

            TaskUtil.Await(() => busControl.StartAsync());

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            log.Info("Stopping bus...");

            busControl?.Stop();

            return true;
        }
    }
}