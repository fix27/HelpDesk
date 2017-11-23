using HelpDesk.Common.EventBus.AppEvents.Interface;

namespace HelpDesk.Common.EventBus.AppEvents
{
    public class RequestDeedlineAppEvent: IRequestDeedlineAppEvent
    {
        public long RequestId { get; set; }
    }
}
