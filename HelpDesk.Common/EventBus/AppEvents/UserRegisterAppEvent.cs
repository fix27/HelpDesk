using System;
using HelpDesk.Common.EventBus.AppEvents.Interface;

namespace HelpDesk.Common.EventBus.AppEvents
{
    public class UserRegisterAppEvent : IUserRegisterAppEvent
    {
        public string Email { get; set; }
    }
}
