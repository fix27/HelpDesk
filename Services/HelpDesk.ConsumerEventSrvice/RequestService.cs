using System;
using System.Configuration;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MassTransit.Util;
using Topshelf;
using Topshelf.Logging;
using HelpDesk.ConsumerEventService.Consumers;
using Unity;
using HelpDesk.Common.EventBus.AppEvents.Interface;

namespace HelpDesk.ConsumerEventService
{
    class RequestService: ServiceControl
    {
        readonly LogWriter log = HostLogger.Get<RequestService>();

        IBusControl busControl;

        public bool Start(HostControl hostControl)
        {
            log.Info("Creating bus...");
            string serviceQueueName = ConfigurationManager.AppSettings["ServiceQueueName"];
            busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                IRabbitMqHost host = x.Host(new Uri(ConfigurationManager.AppSettings["RabbitMQHost"]), h =>
                {
                    h.Username(ConfigurationManager.AppSettings["RabbitMQUserName"]);
                    h.Password(ConfigurationManager.AppSettings["RabbitMQPassword"]);
                });

                x.ReceiveEndpoint(host, serviceQueueName, e => 
                {
                    e.Consumer(typeof(RequestAppEventConsumer), 
                        t => UnityConfig.GetConfiguredContainer().Resolve<IConsumer<IRequestAppEvent>>());

                    e.Consumer(typeof(RequestDeedlineAppEventConsumer),
                        t => UnityConfig.GetConfiguredContainer().Resolve<IConsumer<IRequestDeedlineAppEvent>>());

                    e.Consumer(typeof(UserPasswordRecoveryAppEventConsumer), 
                        t => UnityConfig.GetConfiguredContainer().Resolve<IConsumer<IUserPasswordRecoveryAppEvent>>());

                    e.Consumer(typeof(UserRegisterAppEventConsumer), 
                        t => UnityConfig.GetConfiguredContainer().Resolve<IConsumer<IUserRegisterAppEvent>>());
                });
                
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