using HelpDesk.EventBus.Common.AppEvents.Interface;
using System.Collections.Generic;

namespace HelpDesk.EventBus.Common.AppEvents
{
    public class RequestDeedlineAppEvent: IRequestDeedlineAppEvent
    {
        public IEnumerable<long> RequestIds { get; set; }
    }
}
