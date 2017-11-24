﻿using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.Common.EventBus.AppEvents;
using HelpDesk.Common.EventBus.AppEvents.Interface;

namespace HelpDesk.ConsumerEventSrvice.Consumers
{
    public class UserRegisterAppEventConsumer : IConsumer<IUserRegisterAppEvent>
    {
        readonly ILog log = Logger.Get<UserRegisterAppEventConsumer>();

        public async Task Consume(ConsumeContext<IUserRegisterAppEvent> context)
        {
            log.InfoFormat("Email = {0}", context.Message.Email);

            context.Respond(new UserRegisterAppEvent
            {
                Email = context.Message.Email
            });
        }        
    }
}