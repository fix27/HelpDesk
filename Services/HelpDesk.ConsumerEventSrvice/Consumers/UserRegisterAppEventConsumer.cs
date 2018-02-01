using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.EventBus.Common.AppEvents.Interface;
using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.ConsumerEventService.Sender;
using HelpDesk.ConsumerEventService.Resources;
using System;
using HelpDesk.ConsumerEventService.Handlers;

namespace HelpDesk.ConsumerEventService.Consumers
{
    /// <summary>
    /// Получатель события "Регистрация в личном кабинете"
    /// </summary>
    public class UserRegisterAppEventConsumer : IConsumer<IUserRegisterAppEvent>
    {
        private readonly IAppEventHandler<IUserRegisterAppEvent> handler;
        private readonly ILog log;
        
        public UserRegisterAppEventConsumer(IAppEventHandler<IUserRegisterAppEvent> handler, ILog log)
        {
            this.handler = handler;
            this.log = log;            
        }
        
        public async Task Consume(ConsumeContext<IUserRegisterAppEvent> context)
        {
            log.InfoFormat("UserRegisterAppEventConsumer: Email = {0}", context.Message.Email);
            await handler.Handle(context.Message);
        }
    }
}