using HelpDesk.EventBus.Common.AppEvents.Interface;

namespace HelpDesk.EventBus.Common.AppEvents
{
    public class UserRegisterAppEvent : IUserRegisterAppEvent
    {
        public string Email { get; set; }
    }
}
