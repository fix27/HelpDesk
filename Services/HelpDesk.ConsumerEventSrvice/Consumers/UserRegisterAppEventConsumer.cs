using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.ConsumerEventSrvice.DTO;
using HelpDesk.ConsumerEventSrvice.Sender;
using HelpDesk.ConsumerEventSrvice.Resources;

namespace HelpDesk.ConsumerEventSrvice.Consumers
{
    public class UserRegisterAppEventConsumer : IConsumer<IUserRegisterAppEvent>
    {
        private readonly ILog log;
        private readonly ISender sender;
        public UserRegisterAppEventConsumer(ILog log, ISender sender)
        {
            this.log = log;
            this.sender = sender;
        }
        
        public async Task Consume(ConsumeContext<IUserRegisterAppEvent> context)
        {
            log.InfoFormat("UserRegisterAppEventConsumer: Email = {0}", context.Message.Email);
            await Task.Run(() =>
            {
                sender.Send(new UserEventSubscribeDTO {Email = context.Message.Email }, 
                    Resource.Subject_UserRegisterAppEventConsumer, "UserRegisterAppEvent");
                log.InfoFormat("UserRegisterAppEventConsumer Send OK: Email = {0}", context.Message.Email);
            });
        }        
    }
}