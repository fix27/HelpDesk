using HelpDesk.EventBus.Common.AppEvents.Interface;

namespace HelpDesk.EventBus.Common.AppEvents
{
    public class UserPasswordRecoveryAppEvent: IUserPasswordRecoveryAppEvent
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
