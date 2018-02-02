using System.Threading.Tasks;
using MassTransit.Logging;
using HelpDesk.EventBus.Common.AppEvents.Interface;
using HelpDesk.ConsumerEventService.Sender;
using HelpDesk.ConsumerEventService.DTO;

namespace HelpDesk.ConsumerEventService.Handlers
{
    /// <summary>
    /// Обработчик события "Восстановление пароля"
    /// </summary>
    public class UserPasswordRecoveryAppEventHandler : IAppEventHandler<IUserPasswordRecoveryAppEvent>
    {
        private readonly ILog log;
        private readonly ISender sender;
        public UserPasswordRecoveryAppEventHandler(ILog log, ISender sender)
        {
            this.log = log;
            this.sender = sender;
        }

        public async Task Handle(IUserPasswordRecoveryAppEvent appEvent)
        {
            await sender.SendAsync(new UserPasswordRecoveryAppEventSubscribeDTO
            {
                Email = appEvent.Email,
                Password = appEvent.Password,
                BaseUrl = appEvent.Cabinet ? Program.BaseCabinetUrl : Program.BaseWorkerUrl
            });
            log.InfoFormat("UserPasswordRecoveryAppEventHandler Send OK: Email = {0}", appEvent.Email);
        }
    }
}