using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.Common.EventBus.AppEvents.Interface;
using HelpDesk.ConsumerEventService.Sender;
using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.ConsumerEventService.Resources;

namespace HelpDesk.ConsumerEventService.Consumers
{
    /// <summary>
    /// Получатель события "Восстановление пароля"
    /// </summary>
    public class UserPasswordRecoveryAppEventConsumer : IConsumer<IUserPasswordRecoveryAppEvent>
    {
        private readonly ILog log;
        private readonly ISender sender;
        public UserPasswordRecoveryAppEventConsumer(ILog log, ISender sender)
        {
            this.log = log;
            this.sender = sender;
        }

        public async Task Consume(ConsumeContext<IUserPasswordRecoveryAppEvent> context)
        {
            log.InfoFormat("UserPasswordRecoveryAppEventConsumer: Email = {0}", context.Message.Email);
            await Task.Run(() =>
            {
                sender.Send(new UserPasswordRecoveryAppEventSubscribeDTO
                {
                    Email = context.Message.Email,
                    Password = context.Message.Password
                }, Resource.Subject_UserPasswordRecoveryAppEventConsumer, "UserPasswordRecoveryAppEvent");
                log.InfoFormat("UserPasswordRecoveryAppEventConsumer Send OK: Email = {0}", context.Message.Email);
            });
        }
    }
}