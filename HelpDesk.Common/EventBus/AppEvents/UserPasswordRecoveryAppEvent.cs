using HelpDesk.Common.EventBus.AppEvents.Interface;

namespace HelpDesk.Common.EventBus.AppEvents
{
    public class UserPasswordRecoveryAppEvent: IUserPasswordRecoveryAppEvent
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
