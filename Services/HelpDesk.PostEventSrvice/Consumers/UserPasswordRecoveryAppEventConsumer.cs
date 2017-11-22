using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.Common.EventBus.AppEvents;
using HelpDesk.Common.EventBus.AppEvents.Interface;

namespace HelpDesk.PostEventSrvice.Consumers
{
    public class UserPasswordRecoveryAppEventConsumer : IConsumer<IUserPasswordRecoveryAppEvent>
    {
        readonly ILog log = Logger.Get<RequestAppEventConsumer>();

        public async Task Consume(ConsumeContext<IUserPasswordRecoveryAppEvent> context)
        {
            log.InfoFormat("Email = {0}, Password = {1}", context.Message.Email, context.Message.Password);

            context.Respond(new UserPasswordRecoveryAppEvent
            {
                 Email = context.Message.Email,
                 Password = context.Message.Password
            });
        }        
    }
}