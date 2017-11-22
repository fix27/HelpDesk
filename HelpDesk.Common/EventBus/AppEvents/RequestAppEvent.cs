using HelpDesk.Common.EventBus.AppEvents.Interface;

namespace HelpDesk.Common.EventBus.AppEvents
{
    public class RequestAppEvent: IRequestAppEvent
    {
        public long RequestEventId { get; set; }
    }
}
