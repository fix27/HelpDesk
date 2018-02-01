using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.EventBus.Common.AppEvents.Interface;
using HelpDesk.ConsumerEventService.Sender;
using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.ConsumerEventService.Resources;
using HelpDesk.ConsumerEventService.Handlers;

namespace HelpDesk.ConsumerEventService.Consumers
{
    /// <summary>
    /// Получатель события "Восстановление пароля"
    /// </summary>
    public class UserPasswordRecoveryAppEventConsumer : IConsumer<IUserPasswordRecoveryAppEvent>
    {
        
        private readonly IAppEventHandler<IUserPasswordRecoveryAppEvent> handler;
        private readonly ILog log;

        public UserPasswordRecoveryAppEventConsumer(IAppEventHandler<IUserPasswordRecoveryAppEvent> handler, ILog log)
        {
            this.handler = handler;
            this.log = log;            
        }

        public async Task Consume(ConsumeContext<IUserPasswordRecoveryAppEvent> context)
        {
            log.InfoFormat("UserPasswordRecoveryAppEventConsumer: Email = {0}", context.Message.Email);

            await handler.Handle(context.Message);
        }
    }
}