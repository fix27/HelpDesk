using HelpDesk.EventBus.Common.AppEvents.Interface;

namespace HelpDesk.EventBus.Common.AppEvents
{
    public class RequestAppEvent: IRequestAppEvent
    {
        public long RequestEventId { get; set; }
    }
}
