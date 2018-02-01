using System.Threading.Tasks;
using MassTransit.Logging;
using HelpDesk.EventBus.Common.AppEvents.Interface;
using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.ConsumerEventService.Sender;
using HelpDesk.ConsumerEventService.Resources;
using System;

namespace HelpDesk.ConsumerEventService.Handlers
{
    /// <summary>
    /// Обработчик события "Регистрация в личном кабинете"
    /// </summary>
    public class UserRegisterAppEventHandler : IAppEventHandler<IUserRegisterAppEvent>
    {
        private readonly ILog log;
        private readonly ISender sender;
        public UserRegisterAppEventHandler(ILog log, ISender sender)
        {
            this.log = log;
            this.sender = sender;
        }

        public async Task Handle(IUserRegisterAppEvent appEvent)
        {
            await sender.SendAsync(new UserEventSubscribeDTO { Email = appEvent.Email, BaseUrl = Program.BaseCabinetUrl },
                String.Format(Resource.Subject_UserRegisterAppEventConsumer, Resource.AppName), "UserRegisterAppEvent");
            log.InfoFormat("UserRegisterAppEventHandler Send OK: Email = {0}", appEvent.Email);
        }
    }
}