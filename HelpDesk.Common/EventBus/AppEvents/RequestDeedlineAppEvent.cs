using HelpDesk.Common.EventBus.AppEvents.Interface;
using System.Collections.Generic;

namespace HelpDesk.Common.EventBus.AppEvents
{
    public class RequestDeedlineAppEvent: IRequestDeedlineAppEvent
    {
        public IEnumerable<long> RequestIds { get; set; }
    }
}
